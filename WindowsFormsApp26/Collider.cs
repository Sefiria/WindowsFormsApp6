using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Tooling;

namespace WindowsFormsApp26
{
    public class CollisionsInfo
    {
        public List<CollisionInfo> CollisionInfoList = new List<CollisionInfo>();
        public CollisionsInfo()
        {
        }
        public bool Collides => CollisionInfoList.Count > 0;
    }
    public class CollisionInfo
    {
        public Entity Entity = null;
        public List<int> BoxesIndices;

        public CollisionInfo(Entity entity = null, List<int> boxesIndices = null)
        {
            Entity = entity;
            BoxesIndices = boxesIndices ?? new List<int>();
        }

        public bool Collides => Entity != null && BoxesIndices.Count > 0;
    }

    public class Collider
    {
        public Entity Owner => Core.CurrentEntities.FirstOrDefault(e => e.ID == OwnerID);
        public Guid OwnerID = Guid.Empty;
        public List<Rectangle> LocalBoxes = new List<Rectangle>();
        public List<RectangleF> Boxes => LocalBoxes.Select(box => box.WithOffset(Owner?.Position ?? PointF.Empty).ToF()).ToList();

        public Collider()
        {
        }
        public Collider(Guid ownerID)
        {
            OwnerID = ownerID;
        }
        public Collider(Guid ownerID, byte[] data)
        {
            OwnerID = ownerID;
            ParseData(data);
        }

        public CollisionsInfo GetCollidingEntities(List<Entity> others)
        {
            CollisionsInfo result = new CollisionsInfo();
            if (!Owner.IsCollisionable)
                return result;
            CollisionInfo ci;
            foreach (var other in others)
            {
                if (this == other.Collider || !other.IsCollisionable)
                    continue;
                ci = GetCollisionInfo(other);
                if (ci?.Collides ?? false)
                    result.CollisionInfoList.Add(ci);
            }
            return result;
        }
        private CollisionInfo GetCollisionInfo(Entity other)
        {
            var result = new CollisionInfo(other);
            if (!(Owner?.IsCollisionable ?? false))
                return result;
            int id;
            List<RectangleF> collidingBoxes = GetCollidingBoxes(other.Collider?.Boxes);
            if (collidingBoxes?.Count > 0)
            {
                foreach (var cb in collidingBoxes)
                {
                    id = collidingBoxes.IndexOf(cb);
                    if (!result.BoxesIndices.Contains(id))
                        result.BoxesIndices.Add(id);
                }
            }
            return result;
        }
        public List<RectangleF> GetCollidingBoxes(List<RectangleF> otherBoxes)
        {
            if (otherBoxes == null || !(Owner?.IsCollisionable ?? false))
                return new List<RectangleF>();
            var boxes = Boxes;
            List<RectangleF> result = new List<RectangleF>();
            List<RectangleF> collidingBoxesBuffer = new List<RectangleF>();
            foreach (var box in boxes)
            {
                //collidingBoxesBuffer = otherBoxes.Where(o => box.Contains(o.Location)
                //                                                            || box.Contains(o.Location.PlusF(o.Width, 0f))
                //                                                            || box.Contains(o.Location.PlusF(0f, o.Height))
                //                                                            || box.Contains(o.Location.PlusF((PointF)o.Size)))
                //                                            .ToList();
                collidingBoxesBuffer = otherBoxes.Where(o => box.IntersectsWith(o)).ToList();
                if (collidingBoxesBuffer.Count > 0)
                    result.AddRange(collidingBoxesBuffer);
            }
            return result.Distinct().ToList();
        }

        public bool Collides() => (Owner?.IsCollisionable ?? false) && Core.CurrentEntities.Where(o => o != Owner && o.IsCollisionable).Any(o => Collides(o));
        public bool Collides(List<Entity> others) => others.Any(o => Collides(o));
        public bool Collides(Entity other) => Collides(other.Collider);
        public bool Collides(Collider collider) => Collides(collider?.Boxes);
        public bool Collides(List<RectangleF> otherBoxes) => GetCollidingBoxes(otherBoxes).Count > 0;

        public void ParseData(byte[] data, bool @override = true)
        {
            if (@override)
                LocalBoxes.Clear();

            const uint MINIMUM_VERSION = 0;

            const uint CLDR = ('C') + ('L' << 8) + ('D' << 16) + ('R' << 24);
            //const uint MAIN = ('M') + ('A' << 8) + ('I' << 16) + ('N' << 24);
            //const uint SIZE = ('S') + ('I' << 8) + ('Z' << 16) + ('E' << 24);
            //const uint RECT = ('R') + ('E' << 8) + ('C' << 16) + ('T' << 24);

            using (var ms = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(ms))
                {
                    if (reader.ReadUInt32() != CLDR) return;

                    uint version = reader.ReadUInt32();
                    if (version < MINIMUM_VERSION) return;

                    #region useless
                    //while (reader.PeekChar() != -1)
                    //{
                    //    var chunkId = reader.ReadUInt32();
                    //    var chunkSize = reader.ReadInt32();
                    //    var childChunksSize = reader.ReadInt32();

                    //    var chunkName = System.Text.Encoding.ASCII.GetString(BitConverter.GetBytes(chunkId));

                    //    switch (chunkId)
                    //    {
                    //        case MAIN:
                    //            {
                    //                if (chunkSize != 0) throw new ArgumentException($"[FATAL] Anomaly encountered: chunk '{chunkName}' with has size of '{chunkSize}', expected {0}");
                    //            }
                    //            break;

                    //        case SIZE:
                    //            var sx = reader.ReadInt32();
                    //            var sy = reader.ReadInt32();
                    //            var sz = reader.ReadInt32();
                    //            if (chunkSize != sizeof(int) * 3) throw new ArgumentException($"[FATAL] Anomaly encountered: chunk '{chunkName}' with has size of '{chunkSize}', expected {sizeof(int) * 3}");
                    //            //voxelData = new VoxelDataBytes(new XYZ(sx, sy, sz), DefaultPalette);
                    //            //Models.Add(voxelData);
                    //            break;

                    //        default:
                    //            Console.WriteLine($"Skipping unknown chunk: '{chunkName}'");
                    //            reader.ReadBytes(chunkSize);
                    //            break;
                    //    }
                    //}
                    #endregion

                    uint count = reader.ReadUInt32();
                    for(int i = 0; i < count; i++)
                    {
                        uint x = reader.ReadUInt32();
                        uint y = reader.ReadUInt32();
                        uint w = reader.ReadUInt32();
                        uint h = reader.ReadUInt32();
                        LocalBoxes.Add(new Rectangle((int)x, (int)y, (int)w, (int)h));
                    }
                }
            }
        }
    }
}

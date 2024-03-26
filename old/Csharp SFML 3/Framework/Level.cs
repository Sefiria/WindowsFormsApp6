using Framework.Entities._Entity._Material;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using zelecx;
using static Framework.Warp;

namespace Framework
{
    [Serializable]
    public class Level
    {
        public class TileInfos
        {
            public int x, y;
            public byte[] value;
            public bool[] Trigger;
            public TileInfos(byte _x = 0, byte _y = 0, byte[] _value = null, bool[] _Trigger = null)
            {
                x = _x;
                y = _y;
                if (_value != null)
                {
                    value = _value;
                    Trigger = _Trigger;
                }
                else
                {
                    value = new byte[Tools.MapLayers];
                    for (int i = 0; i < value.Length; i++)
                        value[i] = 0;
                    Trigger = new bool[Tools.MapLayers];
                    for (int i = 0; i < value.Length; i++)
                        Trigger[i] = false;
                }
            }
            public TileInfos Empty { get => new TileInfos(); }
        }
        [Serializable]
        public class AutoTile
        {
            public enum MicrotilePosition { o, p, l, m }

            public byte layer, id;
            public int X, Y;
            public EntityProperties entity;
            public (int o, int p, int l, int m) AutotilesSpritesIDs;
            public bool TrueBackground_FalseEntities;

            public AutoTile(byte _layer, byte _id, int _X, int _Y, EntityProperties _entity, (int o, int p, int l, int m) _AutotilesSpritesIDs, bool _TrueBackground_FalseEntities)
            {
                layer = _layer;
                id = _id;
                X = _X;
                Y = _Y;
                entity = _entity;
                AutotilesSpritesIDs = _AutotilesSpritesIDs;
                TrueBackground_FalseEntities = _TrueBackground_FalseEntities;
            }
            public Sprite GetAutoTileSprite(MicrotilePosition mtp)
            {
                int mtid = -1;
                switch (mtp)
                {
                    case MicrotilePosition.o: mtid = AutotilesSpritesIDs.o; break;
                    case MicrotilePosition.p: mtid = AutotilesSpritesIDs.p; break;
                    case MicrotilePosition.l: mtid = AutotilesSpritesIDs.l; break;
                    case MicrotilePosition.m: mtid = AutotilesSpritesIDs.m; break;
                }
                return SpriteManager.GetSpritesAutoTile(layer, id, mtid);
            }
            public Sprite[] GetAutoTileSprites()
            {
                var result = new Sprite[4];
                result[0] = SpriteManager.GetSpritesAutoTile(layer, id, AutotilesSpritesIDs.o);
                result[1] = SpriteManager.GetSpritesAutoTile(layer, id, AutotilesSpritesIDs.p);
                result[2] = SpriteManager.GetSpritesAutoTile(layer, id, AutotilesSpritesIDs.l);
                result[3] = SpriteManager.GetSpritesAutoTile(layer, id, AutotilesSpritesIDs.m);

                return result;
            }
        }

        public enum CollisionTest { Left, Right, Top, Bottom };

        public byte[,,] map;
        public byte[,] bg;
        public List<AutoTile> autotiles = new List<AutoTile>();
        public byte bgContrast = 255 / 2;
        public List<Door> doors;
        public List<Warp> warps;
        public (int X, int Y) InGameCamera = (0, 0);

        [NonSerialized] public RenderWindow Render = null;
        [NonSerialized] public Rectangle selection = Rectangle.Empty;
        [NonSerialized] public int selectionLayer = 0;
        [NonSerialized] public bool SelectAllLayers = false;
        [NonSerialized] public bool ValidatingPasteSelection = false;
        [NonSerialized] public (int X, int Y) Cam_Request = (-1, -1);
        [NonSerialized] public (int ID, int X, int Y) DoorSelection = (0, -1, -1);
        [NonSerialized] public int WarpSelection = 0;
        [NonSerialized] public bool PlayMode = false;
        [NonSerialized] public Dictionary<EntityProperties, Thread> BehaviorsThread = new Dictionary<EntityProperties, Thread>();
        [NonSerialized] public Entities._Entity._Organic._Playable.Player player = null;
        [NonSerialized] public bool lockDraw = false;
        [NonSerialized] public List<VolatileBlock> volatiles;
        [NonSerialized] public bool anyTileModif = true;
        [NonSerialized] public (float X, float Y) Camera;
        [NonSerialized] public byte[,] ChargeCells;

        public Level()
        {
            doors = new List<Door>();
            warps = new List<Warp>();
            map = new byte[Tools.MapLayers, Tools.MapWidth, Tools.MapHeight];
            bg = new byte[Tools.MapWidth, Tools.MapHeight];
            volatiles = new List<VolatileBlock>();
        }
        public void AbortThreads()
        {
            foreach (var thread in BehaviorsThread)
                thread.Value.Name = "Abort";
        }
        public void InitializeNonSerialized()
        {
            Render = null;
            selection = Rectangle.Empty;
            selectionLayer = 0;
            SelectAllLayers = false;
            ValidatingPasteSelection = false;
            Cam_Request = (-1, -1);
            DoorSelection = (0, -1, -1);
            WarpSelection = 0;
            PlayMode = false;
            BehaviorsThread = new Dictionary<EntityProperties, Thread>();
            volatiles = new List<VolatileBlock>();
            anyTileModif = true;
            ChargeCells = new byte[Tools.MapWidth, Tools.MapHeight];
            for (int x = 0; x < Tools.MapWidth; x++)
                for (int y = 0; y < Tools.MapHeight; y++)
                    ChargeCells[x, y] = 0;
        }
        public void Draw(RenderWindow render, (int X, int Y) Camera, bool[] displayLayer, bool Edit_TrueBackground_FalseEntities, bool PlayMode = false)
        {
            Draw(render, ((float, float))Camera, displayLayer, Edit_TrueBackground_FalseEntities, PlayMode);
        }
        public void Draw(RenderWindow render, (float X, float Y) _Camera, bool[] displayLayer, bool Edit_TrueBackground_FalseEntities, bool PlayMode = false)
        {
            if (lockDraw)
                return;

            Render = render;
            Camera = _Camera;


            var newDict = new Dictionary<EntityProperties, Thread>();
            foreach (var thread in BehaviorsThread)
                if (thread.Value != null)
                    if (thread.Value.Name != "Abort")
                        newDict[thread.Key] = thread.Value;
            BehaviorsThread = newDict;

            Sprite sp;
            bool spSelected = false;
            for (float x = Camera.X - 1; x < Camera.X + Render.Size.X / Tools.TileSize + 1; x++)
            {
                for (float y = Camera.Y - 1; y < Camera.Y + Render.Size.Y / Tools.TileSize + 1; y++)
                {
                    if (x < 0 || y < 0 || x > Render.Size.X || y > Render.Size.Y)
                        continue;

                    for (byte l = 0; l < Tools.MapLayers; l++)
                    {
                        if (map[l, (int)x, (int)y] == 0)
                            continue;
                        if (!displayLayer[l])
                            continue;

                        var entity = Tools.GetEntityPropertyFromID(map[l, (int)x, (int)y]);

                        if (PlayMode)
                        {
                            if (entity.gravityEffect && anyTileModif)
                                if (!CheckCollision(CollisionTest.Bottom, (x * Tools.TileSize, y * Tools.TileSize), Render))
                                {
                                    DeleteTile(l, (int)x, (int)y, false);
                                    volatiles.Add(new VolatileBlock((int)x, (int)y, l, entity.ID));
                                    continue;
                                }
                        }

                        if (entity.autoTiled)
                        {
                            var autotileSprites = autotiles.FirstOrDefault(a => a.layer == l && a.id == map[l, (int)x, (int)y] && a.X == (int)x && a.Y == (int)y)?.GetAutoTileSprites();
                            if (autotileSprites != null)
                                for (int i = 0; i < 2; i++)
                                {
                                    for (int j = 0; j < 2; j++)
                                    {
                                        sp = autotileSprites[i * 2 + j];
                                        if (sp != null)
                                        {
                                            if (!PlayMode)
                                            {
                                                spSelected = selection != Rectangle.Empty && selection.Contains((int)x, (int)y) && (SelectAllLayers || l == selectionLayer) && !Edit_TrueBackground_FalseEntities;
                                                if (spSelected)
                                                {
                                                    var color = Tools.GetColorRainbow();
                                                    sp.Color = new SFML.Graphics.Color(255, (byte)(255 - color.R / 2), (byte)(255 - color.G / 2), (byte)(255 - color.B / 2));
                                                }
                                                if (ValidatingPasteSelection || WarpSelection > 0 || (DoorSelection != (0, -1, -1) && (map[l, (int)x, (int)y], x, y) != DoorSelection))
                                                    sp.Color = new SFML.Graphics.Color(255, 255, 255, 100);
                                            }

                                            sp.Position = new Vector2f(((int)x - Camera.X) * Tools.TileSize + j * Tools.TileSize / 2,
                                                                        ((int)y - Camera.Y) * Tools.TileSize + i * Tools.TileSize / 2);
                                            Render.Draw(sp);
                                            sp.Color = SFML.Graphics.Color.White;
                                        }
                                    }
                                }
                        }
                        else
                        {
                            sp = SpriteManager.GetSprite(l, map[l, (int)x, (int)y]);
                            if (sp != null)
                            {
                                if (!PlayMode)
                                {
                                    spSelected = selection != Rectangle.Empty && selection.Contains((int)x, (int)y) && (SelectAllLayers || l == selectionLayer) && !Edit_TrueBackground_FalseEntities;
                                    if (spSelected)
                                    {
                                        var color = Tools.GetColorRainbow();
                                        sp.Color = new SFML.Graphics.Color(255, (byte)(255 - color.R / 2), (byte)(255 - color.G / 2), (byte)(255 - color.B / 2));
                                    }
                                    if (ValidatingPasteSelection || WarpSelection > 0 || (DoorSelection != (0, -1, -1) && (map[l, (int)x, (int)y], x, y) != DoorSelection))
                                        sp.Color = new SFML.Graphics.Color(255, 255, 255, 100);
                                }

                                sp.Position = new Vector2f(((int)x - Camera.X) * Tools.TileSize, ((int)y - Camera.Y) * Tools.TileSize);
                                Render.Draw(sp);
                                sp.Color = SFML.Graphics.Color.White;
                            }

                            if (PlayMode)
                            {
                                if (entity.behaviorIDToLoad > 0)
                                    entity.behaviorScript = Tools.GetEntityPropertyFromID(entity.behaviorIDToLoad).behaviorScript;
                                if (entity.behaviorScript != null || entity.behaviorIDToLoad > 0)
                                {
                                    if (!string.IsNullOrWhiteSpace(entity.behaviorScript.script))
                                    {
                                        if (!BehaviorsThread.ContainsKey(entity))
                                        {
                                            int Thread_X = (int)x;
                                            int Thread_Y = (int)y;
                                            var thread = new Thread(() => entity.behaviorScript.ScriptMain(this, entity, (int)(Thread_X/*(x - Camera.X) * Tools.TileSize*/), (int)(Thread_Y/*(y - Camera.Y) * Tools.TileSize*/)));
                                            thread.SetApartmentState(ApartmentState.STA);
                                            thread.IsBackground = true;
                                            BehaviorsThread[entity] = thread;
                                            thread.Start();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (!PlayMode)
            {
                foreach (var warp in warps)
                {
                    sp = SpriteManager.GetWarpSprite(warp.type);
                    sp.Position = new Vector2f((warp.TileX - Camera.X) * Tools.TileSize, (warp.TileY - Camera.Y) * Tools.TileSize);
                    if (WarpSelection > 0 && warp.Enter_WarpID != WarpSelection)
                        sp.Color = new SFML.Graphics.Color(255, 255, 255, 100);
                    render.Draw(sp);
                    sp.Color = SFML.Graphics.Color.White;
                }
            }

            foreach (var block in volatiles)
                block.Draw(Render, Camera);

            anyTileModif = false;
        }
        
        public void DrawBackground(RenderWindow render, (int X, int Y) Camera, bool Edit_TrueBackground_FalseEntities)
        {
            if (lockDraw)
                return;

            Sprite sp;
            bool spSelected = false;
            for (int x = Camera.X; x < Camera.X + render.Size.X / Tools.TileSize; x++)
            {
                for (int y = Camera.Y; y < Camera.Y + render.Size.Y / Tools.TileSize; y++)
                {
                    if (x < 0 || y < 0 || x > render.Size.X || y > render.Size.Y)
                        continue;
                    if (bg[x, y] == 0)
                        continue;

                    var entity = Tools.GetEntityPropertyFromID(bg[x, y]);
                    if (entity.autoTiled)
                    {
                        var autotileSprites = autotiles.FirstOrDefault(a => a.id == bg[x, y] && a.X == x && a.Y == y).GetAutoTileSprites();
                        for (int i = 0; i < 2; i++)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                sp = autotileSprites[i * 2 + j];
                                if (sp != null)
                                {
                                    if (sp != null)
                                    {
                                        spSelected = selection != Rectangle.Empty && selection.Contains(x, y) && Edit_TrueBackground_FalseEntities;
                                        if (spSelected)
                                        {
                                            var color = Tools.GetColorRainbow();
                                            sp.Color = new SFML.Graphics.Color((byte)(255 - color.R / 2), (byte)(255 - color.G / 2), (byte)(255 - color.B / 2), bgContrast);
                                        }
                                        else
                                            sp.Color = new SFML.Graphics.Color(255, 255, 255, bgContrast);
                                        sp.Position = new Vector2f(((int)x - Camera.X) * Tools.TileSize + j * Tools.TileSize / 2,
                                                                    ((int)y - Camera.Y) * Tools.TileSize + i * Tools.TileSize / 2);
                                        render.Draw(sp);
                                        sp.Color = SFML.Graphics.Color.White;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        sp = SpriteManager.GetSprite(0, bg[x, y]);
                        if (sp != null)
                        {
                            spSelected = selection != Rectangle.Empty && selection.Contains(x, y) && Edit_TrueBackground_FalseEntities;
                            if (spSelected)
                            {
                                var color = Tools.GetColorRainbow();
                                sp.Color = new SFML.Graphics.Color((byte)(255 - color.R / 2), (byte)(255 - color.G / 2), (byte)(255 - color.B / 2), bgContrast);
                            }
                            else
                                sp.Color = new SFML.Graphics.Color(255, 255, 255, bgContrast);
                            sp.Position = new Vector2f((x - Camera.X) * Tools.TileSize, (y - Camera.Y) * Tools.TileSize);
                            render.Draw(sp);
                            sp.Color = SFML.Graphics.Color.White;
                        }
                    }
                }
            }
        }

        private (int o, int p, int l, int m) GetAutoTiledImageIDs(EntityProperties entity, int layer, int X, int Y, bool Edit_TrueBackground_FalseEntities)
        {
            bool a, z, e, q, d, w, x, c;
            int o = 4, p = 5, l = 6, m = 7;

            bool DetectAutoTile(int i, int j)
            {
                if (i < 0 || i >= Tools.MapWidth || j < 0 || j >= Tools.MapHeight)
                    return false;
                if (Edit_TrueBackground_FalseEntities ? bg[i, j] == 0 : map[layer, i, j] == 0)
                    return false;
                return Tools.GetEntityPropertyFromID(Edit_TrueBackground_FalseEntities ? bg[i, j] : map[layer, i, j]).autoTiled;
            }

            a = DetectAutoTile(X - 1, Y - 1); z = DetectAutoTile(X, Y - 1); e = DetectAutoTile(X + 1, Y - 1);
            q = DetectAutoTile(X - 1, Y); d = DetectAutoTile(X + 1, Y);
            w = DetectAutoTile(X - 1, Y + 1); x = DetectAutoTile(X, Y + 1); c = DetectAutoTile(X + 1, Y + 1);

            #region Microtiles

            #region Microtile_O
            if (a)
            {
                if (z)
                {
                    if (q)
                    {
                        o = 28;
                    }
                    else
                    {
                        o = 24;
                    }
                }
                else
                {
                    if (q)
                    {
                        o = 16;
                    }
                    else
                    {
                        o = 0;
                    }
                }
            }
            else
            {
                if (z)
                {
                    if (q)
                    {
                        o = 8;
                    }
                    else
                    {
                        o = 24;
                    }
                }
                else
                {
                    if (q)
                    {
                        o = 16;
                    }
                    else
                    {
                        o = 0;
                    }
                }
            }
            #endregion

            #region Microtile_P
            if (z)
            {
                if (e)
                {
                    if (d)
                    {
                        p = 29;
                    }
                    else
                    {
                        p = 33;
                    }
                }
                else
                {
                    if (d)
                    {
                        p = 9;
                    }
                    else
                    {
                        p = 33;
                    }
                }
            }
            else
            {
                if (d)
                {
                    p = 17;
                }
                else
                {
                    p = 1;
                }
            }
            #endregion

            #region Microtile_L
            if (q)
            {
                if (w)
                {
                    if (x)
                    {
                        l = 30;
                    }
                    else
                    {
                        l = 42;
                    }
                }
                else
                {
                    if (x)
                    {
                        l = 10;
                    }
                    else
                    {
                        l = 42;
                    }
                }
            }
            else
            {
                if (x)
                {
                    l = 26;
                }
                else
                {
                    l = 2;
                }
            }
            #endregion

            #region Microtile_M
            if (d)
            {
                if (x)
                {
                    if (c)
                    {
                        m = 31;
                    }
                    else
                    {
                        m = 11;
                    }
                }
                else
                {
                    m = 43;
                }
            }
            else
            {
                if (x)
                {
                    m = 35;
                }
                else
                {
                    m = 3;
                }
            }
            #endregion

            #endregion

            return (o, p, l, m);
        }

        public void SetTile(int layer, Point point, byte value, bool Edit_TrueBackground_FalseEntities)
        {
            SetTile(layer, point.X, point.Y, value, Edit_TrueBackground_FalseEntities);
        }
        public void SetTile(int layer, int x, int y, byte value, bool Edit_TrueBackground_FalseEntities)
        {
            if (x < 0 || y < 0 || x >= Tools.MapWidth || y >= Tools.MapHeight)
                return;
            byte prevValue;
            if (Edit_TrueBackground_FalseEntities)
            {
                prevValue = bg[x, y];
                bg[x, y] = value;
            }
            else
            {
                prevValue = map[layer, x, y];
                map[layer, x, y] = value;
                anyTileModif = true;
            }

            CheckTile((byte)layer, x, y, prevValue, value, Edit_TrueBackground_FalseEntities);
            RecalculateAutoTilesIDsAround((byte)layer, x, y, Edit_TrueBackground_FalseEntities);
        }
        public void SetTileIfEmpty(int layer, int x, int y, byte value, bool Edit_TrueBackground_FalseEntities)
        {
            if (x < 0 || y < 0 || x >= Tools.MapWidth || y >= Tools.MapHeight)
                return;
            if (!Edit_TrueBackground_FalseEntities)
            {
                if (map[layer, x, y] == 0)
                {
                    map[layer, x, y] = value;
                    anyTileModif = true;
                }
            }
            else
            {
                if (bg[x, y] == 0)
                    bg[x, y] = value;
            }

            CheckTile((byte)layer, x, y, 0, value, Edit_TrueBackground_FalseEntities);
            RecalculateAutoTilesIDsAround((byte)layer, x, y, Edit_TrueBackground_FalseEntities);
        }
        public void DeleteTile(int layer, Point point, bool Edit_TrueBackground_FalseEntities)
        {
            DeleteTile(layer, point.X, point.Y, Edit_TrueBackground_FalseEntities);
        }
        public void DeleteTile(int layer, int x, int y, bool Edit_TrueBackground_FalseEntities)
        {
            if (x < 0 || y < 0 || x >= Tools.MapWidth || y >= Tools.MapHeight)
                return;

            byte previousID = 0;

            if (!Edit_TrueBackground_FalseEntities)
            {
                previousID = map[layer, x, y];
                map[layer, x, y] = 0;
                anyTileModif = true;
            }
            else
            {
                previousID = bg[x, y];
                bg[x, y] = 0;
            }

            CheckTile((byte)layer, x, y, previousID, 0, Edit_TrueBackground_FalseEntities);
            RecalculateAutoTilesIDsAround((byte)layer, x, y, Edit_TrueBackground_FalseEntities);
        }
        public void PasteRaw(byte[,,] raw, Size size, Point position, bool Edit_TrueBackground_FalseEntities)
        {
            for (int l = 0; l < (SelectAllLayers ? 3 : 1); l++)
            {
                var layer = SelectAllLayers ? l : selectionLayer;
                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        var value = raw[layer, x, y];
                        if (value > 0)
                            SetTile(layer, position.X + x, position.Y + y, value, Edit_TrueBackground_FalseEntities);
                    }
                }
            }
        }
        public void ToggleWarp(WarpType type, int x, int y)
        {
            foreach (var warp in warps)
            {
                if (warp.TilePosition == new Point(x, y))
                {
                    warps.Remove(warp);
                    return;
                }
            }

            warps.Add(new Warp(type, x, y));
        }

        public void CheckTile(byte layer, int x, int y, byte prevValue, byte newValue, bool Edit_TrueBackground_FalseEntities)
        {
            if (x < 0 || x >= Tools.MapWidth || y < 0 || y >= Tools.MapHeight)
                return;

            EntityProperties entity;

            if (newValue > 0)
            {
                if (!Edit_TrueBackground_FalseEntities && !PlayMode)
                {
                    if (IDManager.DoorsID.Contains((layer, newValue)))
                    {
                        var door = new Door(layer, newValue);
                        door.position = (x, y);
                        if (Door.FirstOrDefault(doors, layer, x, y, newValue) == null)
                            doors.Add(door);
                    }
                }

                entity = Tools.GetEntityPropertyFromID(newValue);
                if (entity != null)
                    if (entity.autoTiled)
                        if (!autotiles.Exists(a => a.layer == layer && a.id == entity.ID && a.X == x && a.Y == y && a.TrueBackground_FalseEntities == Edit_TrueBackground_FalseEntities))
                        {
                            autotiles.Add(new AutoTile(layer, entity.ID, x, y, entity, GetAutoTiledImageIDs(entity, layer, x, y, Edit_TrueBackground_FalseEntities), Edit_TrueBackground_FalseEntities));
                            RecalculateAutoTilesIDsAround(layer, x, y, Edit_TrueBackground_FalseEntities);
                        }
            }
            else
            {
                if (!Edit_TrueBackground_FalseEntities && !PlayMode)
                {
                    if (IDManager.DoorsID.Contains((layer, prevValue)))
                    {
                        var door = Door.FirstOrDefault(doors, layer, x, y, prevValue);
                        if (door != null)
                            doors.Remove(door);
                    }
                }

                entity = Tools.GetEntityPropertyFromID(prevValue);
                if (entity != null)
                {
                    if (entity.autoTiled)
                    {
                        autotiles.Remove(autotiles.First(a => a.layer == layer && a.id == prevValue && a.X == x && a.Y == y && a.TrueBackground_FalseEntities == Edit_TrueBackground_FalseEntities));
                        RecalculateAutoTilesIDsAround(layer, x, y, Edit_TrueBackground_FalseEntities);
                    }
                }
            }
        }
        public void RecalculateAutoTilesIDsAround(byte layer, int X, int Y, bool Edit_TrueBackground_FalseEntities)
        {
            EntityProperties entity = null;
            byte id = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    id = GetTileInfosFromPoint(new Point(X + i, Y + j), true, true).value[layer];
                    if (id == 0)
                        continue;
                    entity = Tools.GetEntityPropertyFromID(id);
                    if (entity == null)
                        continue;

                    var autotile = autotiles.FirstOrDefault(a => a.layer == layer && a.id == entity.ID && a.X == X + i && a.Y == Y + j && a.TrueBackground_FalseEntities == Edit_TrueBackground_FalseEntities);
                    if (autotile != null)
                        autotile.AutotilesSpritesIDs = GetAutoTiledImageIDs(entity, layer, X + i, Y + j, Edit_TrueBackground_FalseEntities);
                }
            }
        }

        public TileInfos GetTileInfosFromMouse(RenderWindow render, (int X, int Y) Camera)
        {
            var mouse2i = Mouse.GetPosition(render);
            var mouse = new Point(mouse2i.X, mouse2i.Y);
            return GetTileInfosFromPoint(mouse);
        }
        public TileInfos GetTileInfosFromPoint(Point point, bool alreadyTiled = false, bool alreadyCamera = false)
        {
            if (alreadyTiled)
            {
                if (!alreadyCamera)
                {
                    point.X = (int)((point.X * Tools.TileSize + Camera.X * Tools.TileSize) / Tools.TileSize);
                    point.Y = (int)((point.Y * Tools.TileSize + Camera.Y * Tools.TileSize) / Tools.TileSize);
                }
            }
            else
            {
                point.X = (int)(point.X + (alreadyCamera ? 0 : Camera.X * Tools.TileSize)) / Tools.TileSize;
                point.Y = (int)(point.Y + (alreadyCamera ? 0 : Camera.Y * Tools.TileSize)) / Tools.TileSize;
            }

            var info = new TileInfos();
            info.x = point.X;
            info.y = point.Y;
            for (int l = 0; l < Tools.MapLayers; l++)
            {
                if (info.x < 0 || info.y < 0 || info.x >= Tools.MapWidth || info.y >= Tools.MapHeight)
                    info.value[l] = 0;
                else
                    info.value[l] = map[l, info.x, info.y];

                if(info.value[l] > 0)
                    info.Trigger[l] = Tools.GetEntityPropertyFromID(info.value[l]).trigger;
            }

            return info;
        }

        public Door GetDoor(int x, int y)
        {
            foreach (var door in doors)
                if (door.GetPositionAsPoint() == new Point(x, y))
                    return door;
            return null;
        }
        public Warp GetWarp(int x, int y)
        {
            foreach (var warp in warps)
                if (warp.TilePosition == new Point(x, y))
                    return warp;
            return null;
        }
        public Warp GetWarp(int ID)
        {
            foreach (var warp in warps)
                if (warp.Enter_WarpID == ID)
                    return warp;
            return null;
        }
        public bool WarpEnterIDAlreadyExists(int id)
        {
            foreach (var warpIT in warps)
                if (warpIT.Enter_WarpID == id)
                    return true;
            return false;
        }
        public bool WarpEnterIDAlreadyExists(Warp warp)
        {
            foreach (var warpIT in warps)
                if (warpIT != warp && warpIT.Enter_WarpID == warp.Enter_WarpID)
                    return true;
            return false;
        }
        public int WarpGetAvailableID()
        {
            int id = 0;
            bool found = false;
            while (!found)
            {
                if (!WarpEnterIDAlreadyExists(++id))
                {
                    found = true;
                }
            }
            return id;
        }
        public void EnsureTileVisible(int X, int Y)
        {
            (int X, int Y) pos = ((int)(X - Render.Size.X / Tools.TileSize / 2), (int)(Y - Render.Size.Y / Tools.TileSize / 2));
            if (pos.X < 0) pos.X = 0;
            if (pos.Y < 0) pos.Y = 0;
            if (pos.X > Tools.MapWidth - Render.Size.X / Tools.TileSize) pos.X = (int)(Tools.MapWidth - Render.Size.X / Tools.TileSize);
            if (pos.Y > Tools.MapHeight - Render.Size.Y / Tools.TileSize) pos.Y = (int)(Tools.MapHeight - Render.Size.Y / Tools.TileSize);
            Cam_Request = pos;
        }
        public bool IsTileVisible(int X, int Y)
        {
            return new Rectangle(InGameCamera.X, InGameCamera.Y, InGameCamera.X + (int)Render.Size.X / Tools.TileSize, InGameCamera.Y + (int)Render.Size.Y / Tools.TileSize).Contains(X, Y);
        }
        public bool IsPositionVisible(int X, int Y)
        {
            return new Rectangle(0, 0, (int)Render.Size.X, (int)Render.Size.Y).Contains(X, Y);
        }

        public byte[,,] Copy(Rectangle bounds, bool includeAllLayers, bool Edit_TrueBackground_FalseEntities)
        {
            var raw = new byte[3, bounds.Width, bounds.Height];
            for (int l = 0; l < (includeAllLayers ? 3 : 1); l++)
                for (int x = 0; x < bounds.Width; x++)
                    for (int y = 0; y < bounds.Height; y++)
                    {
                        if (!Edit_TrueBackground_FalseEntities)
                            raw[(includeAllLayers ? l : selectionLayer), x, y] = map[(includeAllLayers ? l : selectionLayer), bounds.Left + x, bounds.Top + y];
                        else
                            raw[0, x, y] = bg[bounds.Left + x, bounds.Top + y];
                    }
            return raw;
        }
        public void RemoveSelection(bool Edit_TrueBackground_FalseEntities)
        {
            for (int l = 0; l < (SelectAllLayers ? 3 : 1); l++)
                for (int x = selection.Left; x < selection.Left + selection.Width; x++)
                    for (int y = selection.Top; y < selection.Top + selection.Height; y++)
                        DeleteTile((SelectAllLayers ? l : selectionLayer), x, y, Edit_TrueBackground_FalseEntities);
        }

        static public Level Load(string path)
        {
            Level level = null;
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                using (Stream stream = File.Open(path, FileMode.Open))
                    level = bf.Deserialize(stream) as Level;
                level.InitializeNonSerialized();
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("Exeption 0x" + e.HResult.ToString("X") + " (HRESULT|FILE_NOT_FOUND)\nCould not find level at path :\n" + Directory.GetCurrentDirectory() + "\\" + path, "Load Level Failed - Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(e.HResult);
            }
            return level;
        }
        
        public void UpdateVolatiles(RenderWindow render)
        {
            var list = new List<VolatileBlock>(volatiles);
            foreach (var block in list)
                block.Update(this, render);
        }
        public void UpdateZelecx(_Main zelecxModule)
        {
            zelecxModule.UpdateCamera((int)Camera.X, (int)Camera.Y);

            zelecxModule.ForEach(
                cell =>
                {
                    if (cell.HasContainer)
                    {
                        if (cell.Container is IExternalOutputInfos)
                        {
                            ChargeCells[cell.X, cell.Y] = (byte)((cell.Container as IExternalOutputInfos).Tension > 0F ? 1 : 0);
                        }
                        if (cell.Container is IExternalInputInfos)
                        {
                            if (ChargeCells[cell.X, cell.Y] == 1)
                                (cell.Container as IExternalInputInfos).Holding = true;
                        }
                    }
                });
        }
        public void Charge(int X, int Y, bool Charged, bool alreadyTiled = false, bool alreadyCamera = false)
        {
            Point point = new Point(X, Y);
            if (alreadyTiled)
            {
                if (!alreadyCamera)
                {
                    point.X = (int)((point.X * Tools.TileSize + Camera.X * Tools.TileSize) / Tools.TileSize);
                    point.Y = (int)((point.Y * Tools.TileSize + Camera.Y * Tools.TileSize) / Tools.TileSize);
                }
            }
            else
            {
                point.X = (int)(point.X + (alreadyCamera ? 0 : Camera.X * Tools.TileSize)) / Tools.TileSize;
                point.Y = (int)(point.Y + (alreadyCamera ? 0 : Camera.Y * Tools.TileSize)) / Tools.TileSize;
            }

            ChargeCells[point.X, point.Y] = (byte)(Charged ? 1 : 0);
        }

        public bool CheckCollision(CollisionTest where, (float X, float Y) position, RenderWindow render)
        {
            bool blocked = false;
            var point = Point.Empty;

            for (int i = 1; i < Tools.TileSize && !blocked; i++)
            {
                switch (where)
                {
                    case CollisionTest.Left: point = new Point((int)position.X - 1, (int)position.Y + i); break;
                    case CollisionTest.Right: point = new Point((int)position.X + Tools.TileSize, (int)position.Y + i); break;
                    case CollisionTest.Top: point = new Point((int)position.X + i, (int)position.Y - 1); break;
                    case CollisionTest.Bottom: point = new Point((int)position.X + i, (int)position.Y + Tools.TileSize); break;
                }

                var info = GetTileInfosFromPoint(point, false, true);
                for (int l = 0; l < Tools.MapLayers && !blocked; l++)
                    if (info.value[l] != 0 && !info.Trigger[l])
                        blocked = true;
            }

            return blocked;
        }
    }
}

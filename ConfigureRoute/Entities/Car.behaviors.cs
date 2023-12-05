using ConfigureRoute.Obj;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Tooling;
using static ConfigureRoute.Enumerations;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ConfigureRoute.Entities
{
    public partial class Car : Entity
	{
		/// <summary>
		/// Evènement, tache sur laquelle la car va focus à chaque frame jusqu'à accomplissement.
		/// </summary>
		private class Event
		{
			public EventSubject subject;
			public float OverPrio = 1F;
			public Road road;
			public Sign sign;
            public Event(EventSubject subject, Road road = null, Sign sign = null)
            {
				this.subject = subject;
				this.road = road;
				this.sign = sign;
            }
        }

		List<Road> roads => Core.Map.Roads;
		/// <summary>
		/// Le Focus est l'évènement sur lequel la car se concentre à chaque frame, c'est l'objectif à atteindre.
		/// </summary>
		Event Focus;

		/// <summary>
		/// Designé pour définir, pour chaque nouvel event, si le nouveau est prioritaire sur le courant.
		/// </summary>
		/// <param name="focus"></param>
		void PriorFocus(Event focus)
		{
			if(Focus == null)
			{
				Focus = focus;
				return;
			}
			var current = (int)Focus.subject;
			var @new = (int)focus.subject;
			if (current > @new) Focus = focus;
			if (current == @new)
			{
				var current_over = Focus.OverPrio;
				var new_over = focus.OverPrio;
				if (current_over >= new_over) Focus = focus;// si parts égales aussi, le nouveau sera prio (nouvelle entrée)
				// sinon fallback : le Focus ne change pas
			}
			// sinon fallback : le Focus ne change pas
		}
		void RemoveCurrentFocus()
		{
			Focus = null;
		}

		void Analyse_DistanceAvecVoitureDevant(Data data)
		{
			if(Status == DrivingStatus.Decelerate)
				Status = DrivingStatus.Accelerate;
			if (Speed.Value.Round(3) == 0)
				return;

			float d;
			Car str8 = null;
            int hit_index = Maths.SimpleRaycastHit(Pos, data.look, Speed.Value, Core.Cube * 2F, Core.Map.CarsExcept(this).Select(c => c.BoundsF).ToList());
			if (hit_index >= 0)
			{
				str8 = Core.Map.Cars[hit_index];
				d = Maths.Distance(Pos, str8.Pos);
			}
			else
			{
				var pos = Pos.PlusF(data.look.x(Core.Cube));
                if (Road.At(pos) == null)
				{
					d = Core.Cube;
				}
				else
					return;
			}

			float min;
			if(str8 != null)
				min = Regulation.Minimum_Distance_Between_Cars + Diagonal / 2F + Maths.Diagonal((int)str8.W, (int)str8.H) / 2F;
			else
				min = Regulation.Minimum_Distance_Between_Cars + Diagonal / 2F + Maths.Diagonal(Core.Cube, Core.Cube) / 2F;

            if (d < min / 4F)
				Status = DrivingStatus.HardBrake;
			else if (d < min / 2F)
				Status = DrivingStatus.Brake;
			else if (d < min || str8?.Speed.Value < Speed.Value)
				Status = DrivingStatus.Decelerate;
		}
		void Analyse_ResteBienDansSaVoie(Data data)
		{
			float x_road, y_road, x_car, y_car, gap;
			const float gap_max = 4F;
			if (data.road.Direction == UP || data.road.Direction == DOWN)
			{
				x_road = data.road.WorldX + Core.Cube / 2;
				x_car = Pos.X;
				gap = x_road - x_car;
				if (gap < -gap_max) Pos = Pos.Minus(Speed.Value, 0F);
				else if (gap > gap_max) Pos = Pos.PlusF(Speed.Value, 0F);
			}
			else
			{
				y_road = data.road.WorldY + Core.Cube / 2;
				y_car = Pos.Y + Diagonal / 2F;
                gap = y_road - y_car;
				if (gap < -gap_max) Pos = Pos.Minus(0F, Speed.Value);
				else if (gap > gap_max) Pos = Pos.PlusF(0F, Speed.Value);
			}
		}
		void Analyse_VerifieSiDoitBientotTournerPourRalentir(Data data)
		{
		}
        void Analyse_TourneSensRoute(Data data)
		{
			float turnAmount = GetAngleGapWithRoad();
			if (turnAmount != 0)
				Angle.Value += Maths.Sign(turnAmount) * 5F;
		}
		void Analyse_VerifieCederPassage(Data data)
		{
			bool check(Road road)
			{
				if (road == null) return true;
				Sign sign = road.Direction == DOWN ? road.Next(0, 1)?.Sign : (road.Direction == RIGHT ? road.Next(1, 0)?.Sign : road.Sign);
				if ((sign != null &&
				  ((road.Direction == UP && sign.t == Sign.CederLePassage)
				|| (road.Direction == LEFT && sign.l == Sign.CederLePassage)))
				|| (road.Direction == DOWN && road.Next(0, 1)?.Sign?.t == Sign.CederLePassage)
				|| (road.Direction == RIGHT && road.Next(1, 0)?.Sign?.l == Sign.CederLePassage)
				)
				{
					PriorFocus(new Event(EventSubject.CederLePassage, road, sign));
					return true;
				}
				return false;
			}

			List<Road> routes_voie = new List<Road>();
			if (Visibility >= 1) routes_voie.Add(data.road);
			Road r = data.road;
			for(int i=1; i<=Visibility; i++)
			{
				r = r?.NextFromZ();
				if (r == null)
					break;
				routes_voie.Add(r);
			}

			// on n'observe pas plus loin si on trouve, la prio est le plus près
			foreach (var route in routes_voie)
				if (check(route))
					break;
		}




		void DoFocus(Data data)
		{
			if (Focus?.road == data.road?.Behind())
				Focus = null;

            switch(Focus?.subject)
			{
				case EventSubject.Accident: DoFocus_Accident(data); break;
				case EventSubject.RegarderGaucheDroite: DoFocus_RegarderGaucheDroite(data); break;
				case EventSubject.CederLePassage: DoFocus_CederLePassage(data); break;
			}
		}
		void DoFocus_Accident(Data data)
		{
		}
		void DoFocus_CederLePassage(Data data)
		{
			if(Maths.Abs(Focus.road.x - data.road.x) + Maths.Abs(Focus.road.y - data.road.y) < 2.5F)
				Status = Focus?.road == data.road ? DrivingStatus.HardBrake : DrivingStatus.Brake;
			else
				Status = DrivingStatus.Decelerate;

			SetStopWhenNoSpeed();

			if(Status == DrivingStatus.Stop)
			{
				if (Maths.Distance(Focus.sign.Position.pt.x(Core.Cube), Pos) > H)
					PosAddLook(CalculateLook(), 0.2F);
				else
					PriorFocus(new Event(EventSubject.RegarderGaucheDroite));
			}
		}
		void DoFocus_RegarderGaucheDroite(Data data)
		{
			var t = Tile.Plus(data.look);
			var road = Core.Map.FindRoad(t.X, t.Y);
			if (road == null) return;
			var car_direction = data.look.AsDirection();
			var atleft = road.NextFromV(car_direction.RotateDirection(Ways.AGauche));
			var atright = road.NextFromV(car_direction.RotateDirection(Ways.ADroite));

			if (road.GetCarsOnIt(this).Any()) return;
			if (road.Direction != car_direction.RotateDirection(Ways.AGauche) && (atleft?.GetCarsOnIt(this).Any() ?? false)) return;
			if (road.Direction != car_direction.RotateDirection(Ways.ADroite) && (atright?.GetCarsOnIt(this).Any() ?? false)) return;
			
			RemoveCurrentFocus();
			Status = DrivingStatus.Accelerate;
			PosAddLook(data.look, 2F);
		}
	}
}

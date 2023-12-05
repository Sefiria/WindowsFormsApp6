using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureRoute
{
    public class Enumerations
    {
		public const int UP = (int)Directions.Up, TOP = (int)Directions.Top, LEFT = (int)Directions.Left, DOWN = (int)Directions.Down, BOTTOM = (int)Directions.Bottom, RIGHT = (int)Directions.Right;

        public enum Directions
        {
            Up = 0, Top = 0, Left = 1, Down = 2, Bottom = 2, Right = 3
        }
        public enum RouteTools
        {
            Road = 0, Sign
        }
        public enum Ways
        {
            Devant = 0,
            AGauche,
            Derriere,
            ADroite,
        }
        public enum EventSubject
        {
            // la priorité absolue est la 0, plus la valeur de l'enum est grande moins c'est prioritaire
            Accident = 0,
            RegarderGaucheDroite,
            CederLePassage,
        }
        public enum DrivingStatus
        {
            Stop = 0,
            HardBrake,
            Brake,
            Decelerate,
            Accelerate,
            Hold,
        }
    }
}

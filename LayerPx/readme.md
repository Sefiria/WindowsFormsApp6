
            if(IsKeyDown(Key.LeftCtrl))
            {
                if (IsKeyPressed(Key.Left)) mirror_mode = (MirrorMode)Maths.Range(0, Enum.GetNames(typeof(MirrorMode)).Count() - 1, (int)mirror_mode - 1);
                if (IsKeyPressed(Key.Right)) mirror_mode = (MirrorMode)Maths.Range(0, Enum.GetNames(typeof(MirrorMode)).Count() - 1, (int)mirror_mode + 1);
            }
            else
            {
                if (IsKeyPressed(Key.Left)) Mode = (ToolModes)Maths.Range(0, Enum.GetNames(typeof(ToolModes)).Count() - 1, (int)Mode - 1);
                if (IsKeyPressed(Key.Right)) Mode = (ToolModes)Maths.Range(0, Enum.GetNames(typeof(ToolModes)).Count() - 1, (int)Mode + 1);
            }

LCTRL + LALT + LSHIFT + C : Clear<br>

[LSHIFT for faster]<br>
Z : Move Cam Up<br>
Q : Move Cam Left<br>
S : Move Cam Down<br>
D : Move Cam Right<br>

G : Show/Hide Grid<br>
H : Show/Hide Sun and Mirror<br>
J : Show/Hide Shadows<br>

If Show Sun and Mirror enabled [H] :<br>
(numpad) 8 : Move mirror source Up<br>
(numpad) 4 : Move mirror source Left<br>
(numpad) 2 : Move mirror source Down<br>
(numpad) 6 : Move mirror source Right<br>
When Holding [LCTRL] :<br>
(numpad) 8 : Move sun source Up<br>
(numpad) 4 : Move sun source Left<br>
(numpad) 2 : Move sun source Down<br>
(numpad) 6 : Move sun source Right<br>

P : Switch between Square Pen & Circle Pen<br>

(arrow) Left / Right : Change Draw Mode<br>
LCTRL + (arrow) Left / Right : Change Mirror Mode<br>

(arrow) Up / Down : Change Fixed Layer<br>
LCTRL + (arrow) Up / Down : Change Layer Gap (quantity of layers to add/substract to fixed layer (when holding [LCTRL])<br>

(mouse) Middle : Eyedrop Color Index & Fixed Layer<br>
(mouse) Scroll Up / Down : Change Pen Size<br>
LALT + (mouse) Scroll Up / Down : Change Zoom<br>
(mouse) Left : Draw<br>
(mouse) Right : Erase (set layer 0, all layers indexes 0)<br>

LCTRL + C : Export to Clipboard (only output image, keep actual zoom for perfect scaling)<br>
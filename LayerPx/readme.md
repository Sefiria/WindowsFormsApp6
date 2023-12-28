# CONTROLS

## MISC

- LCTRL + LALT + LSHIFT + C : Clear<br>

## CAM

[<b>LSHIFT for faster</b>]<br>
- Z : Move Cam Up<br>
- Q : Move Cam Left<br>
- S : Move Cam Down<br>
- D : Move Cam Right<br>

## OPTIONS

- G : Show/Hide Grid<br>
- H : Show/Hide Sun and Mirror<br>
- J : Show/Hide Shadows<br>

## MIRROR & SUN

[<b>LSHIFT for faster</b>]<br>

<u>If Show Sun and Mirror enabled [H]</u> :<br>
- <i>(numpad)</i> 8 : Move mirror source Up<br>
- <i>(numpad)</i> 4 : Move mirror source Left<br>
- <i>(numpad)</i> 2 : Move mirror source Down<br>
- <i>(numpad)</i> 6 : Move mirror source Right<br>

<u>When Holding [LCTRL]</u> :<br>
- <i>(numpad)</i> 8 : Move sun source Up<br>
- <i>(numpad)</i> 4 : Move sun source Left<br>
- <i>(numpad)</i> 2 : Move sun source Down<br>
- <i>(numpad)</i> 6 : Move sun source Right<br>

## TOOLS

- P : Switch between Square Pen & Circle Pen<br>
- <i>(mouse)</i> Middle : Eyedrop Color Index & Fixed Layer<br>
- [<b>LSHIFT for faster</b>] <i>(mouse)</i> Scroll Up / Down : Change Pen Size<br>
- <i>(mouse)</i> Right : Erase (set layer 0, all layers indexes 0)<br>
[<b>LSHIFT for bucket tool</b>]<br>
- <i>(mouse)</i> Left : Draw / Modify layer<br>
- <i>holding</i> LALT : Create a Line (<i>(mouse)</i> Left to draw it)
- <i>holding</i> LALT & LCtrl : Create a Circle (<i>(mouse)</i> Left to draw it)

## DRAW & MIRROR MODES

- <i>(arrow)</i> Left / Right : Change Draw Mode<br>
- LCTRL + <i>(arrow)</i> Left / Right : Change Mirror Mode<br>

## LAYERS

- <i>(arrow)</i> Up / Down : Change Fixed Layer<br>
- LCTRL + <i>(arrow)</i> Up / Down : Change Layer Gap (quantity of layers to add/substract to fixed layer (when holding [LCTRL])<br>

## VIEW

- LALT + <i>(arrow)</i> Scroll Up / Down : Change Zoom<br>

## IMPORT & EXPORT

- LCTRL + C : Export to Clipboard (only output image, keep actual zoom for perfect scaling)<br>
<br>
<br>
# HOW TO

## Draw Modes

- <u>N (Normal)</u> :<br>
Draw only at fixed layer (doesn't affect others layers)
- <u>F (Force)</u> :<br>
Draw but overwriting the visible layer (for the value on the fixed layer, remove value from others layers)
- <u>A (Auto)</u> :<br>
Automatically increase/decrease the pointed layer setting also the value.<br>
<i>(mouse)</i> Left : increase<br>
[LCTRL] + <i>(mouse)</i> Left : decrease<br>
- <u>U (Up)</u> :<br>
Increase <b>only</b> the pointed layer, set the value.<br>
- <u>D (Down)</u> :<br>
Decrease <b>only</b> the pointed layer, set the value.<br>

## Mirror Modes

Use the mirror source (purple cross, use [<b>H</b>] to show/hide) for symetrical draw.<br>
- <u>_ (None)</u> :<br>
No mirror
- <u>X</u> :<br>
X mirror (symetric draw on X)
- <u>Y</u> :<br>
Y mirror (symetric draw on Y)
- <u>XY</u> :<br>
X & Y mirrors (symetric draw on X and Y)<br>

## Sun & Shadows

Use [<b>H</b>] to show/hide Sun & Mirror sources.<br>
The sun source will apply on the center of the output area.
Use LCTRL + <i>(numpad)</i> 8, 4, 2, 6 to move the sun source.
By default the sun source is at center point (no shadow).
Shadows applies automatically related to the sun vector (source to output's center).

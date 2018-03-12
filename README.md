# Unity3D-ui-event-state-tool
Add pointer event functionality to all of your UI components!

Introduction
The User Interface revamp that Unity received in version 4.6 is incredible, and at its’ time groundbreaking. It became easy and intuitive to create UI for games. But despite the great advances, there are definitely some limitations. For instance, Button elements can only capture certain events. Moreover, you can also only change the element’s color and you only have an option of using a callback with the PointerOnClick event. While this is okay for a basic mock-up, anything further would require custom code and a lot of time from designers and developers.

Meet the Unity3D UIGraphicEventState Tool
When applied to a UI Component that has a Graphic component attached (Image or Text), 10 event states become available! Moreover, each Event State allows you to choose any or all of seven transitions to help get your message across right the first time.

Pointer Events
Pointer Events are captured by directly interacting with an element. Each event lasts for a default of 0.25 seconds, but this can be changed to fit your needs.

The ten captured Pointer Events are:
OnNormal – The default state when there is no interaction with the element
OnHover – Pointer is inside the element’s region
OnPointerEnter – The moment the pointer entered the element’s region
OnPointerExit – The moment the pointer left the element’s region
OnBeginDrag – The pointer was in a “pressed” state and dragged over an element’s region
OnDrag – The pointer is in a “pressed” state and is dragging over an element’s region
OnEndDrag – The pointer was released after being in the “OnDrag” state
OnPointerDown – The pointer was pressed “down” while inside an element’s region
OnPointerUp – The pointer was released after being pressed down inside an element’s region
OnPointerClick – The pointer was pressed and released inside of an element’s region
Transitions
A Transition is started when a Pointer Event is started, and ends when a Pointer Event ends. Transitions can be referred to as event callbacks.

The seven available Transitions are:
Color – Adjusts the element’s Text or Image color over the event’s duration
Material – Adjusts the graphic’s material
Rotation – Adjusts the element’s world-space rotation over the event’s duration
Scale – Adjusts the element’s scale over the event’s duration
SpriteAndFill – Adjusts the Image element’s sprite, and fills it over the event’s duration (Image element only)
TextFontSize – Adjusts the Text element’s Font Size property over the event’s duration (Text element only)
TextWriteout – Adjusts the Text element’s Text property. This can be shown all at once or written out over the event’s duration (Text element only)
Ease of Use
The UI Graphic Event State tool is extremely easy to use. To make your life even easier, I created a Custom Inspector that will allow you set each state. Moreover, you can preview each Event state and Transition by clicking the “Set current” and “Set previous” buttons:

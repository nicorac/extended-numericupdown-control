# Extended NumericUpDownControl

Extension to .NET NumericUpDown control that adds new features and fixes:

* fix original MouseEnter and MouseLeave events
* new property InterceptMouseWheel: Always/WhenMouseOver/Never
* show up/down buttons only when mouse is over the control
* show up/down buttons only when the control has focus (regardless of mouseover)
* new events BeforeValueDecrement/BeforeValueIncrement to allow giving different increment/decrement steps based on current control value 

See documentation in file /Doc/index.html for further details.
' ===================================================================== '
' NumericUpDownEx - v.1.6                                               '
' ===================================================================== '
' Author:   Claudio Nicora                                              '
' WebSite:  http://coolsoft.altervista.org/numericupdownex              '
' CodeProject: http://www.codeproject.com/KB/edit/NumericUpDownEx.aspx  '
' License:  CodeProject Open License                                    '
'           http://www.codeproject.com/info/cpol10.aspx                 '
' Feel free to contribute here: http://coolsoft.altervista.org          '
' ===================================================================== '

Imports System.Drawing
Imports System.ComponentModel
Imports System.Windows.Forms

<DesignerCategory("code")>
Public Class NumericUpDownEx

    Inherits NumericUpDown

    ' reference to the underlying TextBox control
    Private _textbox As TextBox

    ' reference to the underlying UpDownButtons control
    Private _upDownButtons As Control


    ''' <summary>
    ''' object creator
    ''' </summary>
    Public Sub New()
        MyBase.New()
        ' get a reference to the underlying UpDownButtons field
        ' Underlying private type is System.Windows.Forms.UpDownBase+UpDownButtons
        _upDownButtons = MyBase.Controls(0)
        If _upDownButtons Is Nothing OrElse _upDownButtons.GetType().FullName <> "System.Windows.Forms.UpDownBase+UpDownButtons" Then
            Throw New ArgumentNullException(Me.GetType.FullName & ": Can't get a reference to internal UpDown buttons field.")
        End If
        ' Get a reference to the underlying TextBox field.
        ' Underlying private type is System.Windows.Forms.UpDownBase+UpDownButtons
        _textbox = TryCast(MyBase.Controls(1), TextBox)
        If _textbox Is Nothing OrElse _textbox.GetType().FullName <> "System.Windows.Forms.UpDownBase+UpDownEdit" Then
            Throw New ArgumentNullException(Me.GetType.FullName & ": Can't get a reference to internal TextBox field.")
        End If
        ' add handlers (MouseEnter and MouseLeave events of NumericUpDown
        ' are not working properly)
        AddHandler _textbox.MouseEnter, AddressOf _mouseEnterLeave
        AddHandler _textbox.MouseLeave, AddressOf _mouseEnterLeave
        AddHandler _upDownButtons.MouseEnter, AddressOf _mouseEnterLeave
        AddHandler _upDownButtons.MouseLeave, AddressOf _mouseEnterLeave
        AddHandler MyBase.MouseEnter, AddressOf _mouseEnterLeave
        AddHandler MyBase.MouseLeave, AddressOf _mouseEnterLeave
    End Sub


    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        If _upDownButtons.Visible = False Then
            e.Graphics.Clear(Me.BackColor)
        End If
        MyBase.OnPaint(e)
    End Sub


    ''' <summary>
    ''' WndProc override to kill WN_MOUSEWHEEL message
    ''' </summary>
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Const WM_MOUSEWHEEL As Integer = &H20A

        If m.Msg = WM_MOUSEWHEEL Then
            Select Case InterceptMouseWheel
                Case InterceptMouseWheelMode.Always
                    ' standard message
                    MyBase.WndProc(m)
                Case InterceptMouseWheelMode.WhenMouseOver
                    If _mouseOver Then
                        ' standard message
                        MyBase.WndProc(m)
                    End If
                Case InterceptMouseWheelMode.Never
                    ' kill the message
                    Exit Sub
            End Select
        Else
            MyBase.WndProc(m)
        End If

    End Sub


#Region " New properties "

    <DefaultValue(False)>
    <Category("Behavior")>
    <Description("Automatically select control text when it receives focus.")>
    Public Property AutoSelect() As Boolean


    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property SelectionStart() As Integer
        Get
            Return _textbox.SelectionStart
        End Get
        Set(ByVal value As Integer)
            _textbox.SelectionStart = value
        End Set
    End Property


    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property SelectionLength() As Integer
        Get
            Return _textbox.SelectionLength
        End Get
        Set(ByVal value As Integer)
            _textbox.SelectionLength = value
        End Set
    End Property


    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property SelectedText() As String
        Get
            Return _textbox.SelectedText
        End Get
        Set(ByVal value As String)
            _textbox.SelectedText = value
        End Set
    End Property


    <Category("Behavior")>
    <DefaultValue(GetType(InterceptMouseWheelMode), "Always")>
    <Description("Enables MouseWheel only under certain conditions.")>
    Public Property InterceptMouseWheel() As InterceptMouseWheelMode = InterceptMouseWheelMode.Always


    Public Enum InterceptMouseWheelMode
        ''' <summary>MouseWheel always works (defauld behavior)</summary>
        Always
        ''' <summary>MouseWheel works only when mouse is over the (focused) control</summary>
        WhenMouseOver
        ''' <summary>MouseWheel never works</summary>
        Never
    End Enum


    <Category("Behavior")>
    <DefaultValue(GetType(ShowUpDownButtonsMode), "Always")>
    <Description("Set UpDownButtons visibility mode.")>
    Public Property ShowUpDownButtons() As ShowUpDownButtonsMode
        Get
            Return _showUpDownButtons
        End Get
        Set(ByVal value As ShowUpDownButtonsMode)
            _showUpDownButtons = value
            ' update UpDownButtons visibility
            UpdateUpDownButtonsVisibility()
        End Set
    End Property
    Private _showUpDownButtons As ShowUpDownButtonsMode = ShowUpDownButtonsMode.Always


    Public Enum ShowUpDownButtonsMode
        ''' <summary>UpDownButtons are always visible (defauld behavior)</summary>
        Always
        ''' <summary>UpDownButtons are visible only when mouse is over the control</summary>
        WhenMouseOver
        ''' <summary>UpDownButtons are visible only when control has the focus</summary>
        WhenFocus
        ''' <summary>UpDownButtons are visible when control has focus or mouse is over the control</summary>
        WhenFocusOrMouseOver
        ''' <summary>UpDownButtons are never visible</summary>
        Never
    End Enum


    ''' <summary>
    ''' If set, incrementing value will cause it to restart from Minimum 
    ''' when Maximum is reached (and viceversa).
    ''' </summary>
    <DefaultValue(False)>
    <Category("Behavior")>
    <Description("If set, incrementing value will cause it to restart from Minimum when Maximum is reached (and viceversa).")>
    Public Property WrapValue() As Boolean

#End Region


#Region " Text selection "

    ''' <summary>
    ''' Select all the text on focus enter
    ''' </summary>
    Protected Overrides Sub OnGotFocus(ByVal e As System.EventArgs)
        _haveFocus = True
        If AutoSelect Then
            _textbox.SelectAll()
        End If
        ' Update UpDownButtons visibility
        If _showUpDownButtons = ShowUpDownButtonsMode.WhenFocus _
                OrElse _showUpDownButtons = ShowUpDownButtonsMode.WhenFocusOrMouseOver Then
            UpdateUpDownButtonsVisibility()
        End If
        MyBase.OnGotFocus(e)
    End Sub


    ''' <summary>
    ''' Indicate that we have lost the focus
    ''' </summary>
    Protected Overrides Sub OnLostFocus(e As EventArgs)
        _haveFocus = False
        ' Update UpDownButtons visibility
        If _showUpDownButtons = ShowUpDownButtonsMode.WhenFocus _
                OrElse _showUpDownButtons = ShowUpDownButtonsMode.WhenFocusOrMouseOver Then
            UpdateUpDownButtonsVisibility()
        End If
        MyBase.OnLostFocus(e)
    End Sub


    ''' <summary>
    ''' MouseUp will kill the SelectAll made on GotFocus.
    ''' Will restore it, but only if user have not made a partial text selection.
    ''' </summary>
    Protected Overrides Sub OnMouseUp(ByVal mevent As System.Windows.Forms.MouseEventArgs)
        If AutoSelect AndAlso _textbox.SelectionLength = 0 Then
            _textbox.SelectAll()
        End If
        MyBase.OnMouseUp(mevent)
    End Sub

#End Region


#Region " Additional events "

    ' these events will be raised correctly, when mouse enters on the textbox
    Shadows Event MouseEnter As EventHandler(Of EventArgs)
    Shadows Event MouseLeave As EventHandler(Of EventArgs)

    ' Events raised BEFORE value decrement/increment
    Public Event BeforeValueDecrement As CancelEventHandler
    Public Event BeforeValueIncrement As CancelEventHandler

    ' flag to track mouse position
    Private _mouseOver As Boolean = False

    ' flag to track focus
    Private _haveFocus As Boolean = False

    ' this handler is called at each mouse Enter/Leave movement
    Private Sub _mouseEnterLeave(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim cr As Drawing.Rectangle = RectangleToScreen(ClientRectangle)
        Dim mp As Drawing.Point = MousePosition

        ' actual state
        Dim isOver As Boolean = cr.Contains(mp)

        ' test if status changed
        If _mouseOver Xor isOver Then
            ' update state
            _mouseOver = isOver
            If _mouseOver Then
                RaiseEvent MouseEnter(Me, EventArgs.Empty)
            Else
                RaiseEvent MouseLeave(Me, EventArgs.Empty)
            End If
        End If

        ' update UpDownButtons visibility
        If _showUpDownButtons <> ShowUpDownButtonsMode.Always Then
            UpdateUpDownButtonsVisibility()
        End If

    End Sub

#End Region


#Region " Value increment/decrement management "

    ' raises the two new events
    Public Overrides Sub DownButton()
        If MyBase.ReadOnly Then Exit Sub
        Dim e As New CancelEventArgs
        RaiseEvent BeforeValueDecrement(Me, e)
        If e.Cancel Then Exit Sub
        ' decrement with wrap
        If WrapValue AndAlso Value - Increment < Minimum Then
            Value = Maximum
        Else
            MyBase.DownButton()
        End If
    End Sub
    Public Overrides Sub UpButton()
        If MyBase.ReadOnly Then Exit Sub
        Dim e As New CancelEventArgs
        RaiseEvent BeforeValueIncrement(Me, e)
        If e.Cancel Then Exit Sub
        ' increment with wrap
        If WrapValue AndAlso Value + Increment > Maximum Then
            Value = Minimum
        Else
            MyBase.UpButton()
        End If
    End Sub

#End Region


#Region " UpDownButtons visibility management "

    ''' <summary>
    ''' Show or hide the UpDownButtons, according to ShowUpDownButtons property value
    ''' </summary>
    Sub UpdateUpDownButtonsVisibility()

        ' test new state
        Dim newVisible As Boolean = False
        Select Case _showUpDownButtons
            Case ShowUpDownButtonsMode.WhenMouseOver
                newVisible = _mouseOver
            Case ShowUpDownButtonsMode.WhenFocus
                newVisible = _haveFocus
            Case ShowUpDownButtonsMode.WhenFocusOrMouseOver
                newVisible = _haveFocus OrElse _mouseOver
            Case ShowUpDownButtonsMode.Never
                newVisible = False
            Case Else
                newVisible = True
        End Select

        ' assign only if needed
        If _upDownButtons.Visible <> newVisible Then
            If newVisible Then
                _textbox.Width = Me.ClientRectangle.Width - _upDownButtons.Width
            Else
                _textbox.Width = Me.ClientRectangle.Width
            End If
            _upDownButtons.Visible = newVisible
            OnTextBoxResize(_textbox, EventArgs.Empty)
            Me.Invalidate()
        End If

    End Sub


    ''' <summary>
    ''' Custom textbox size management
    ''' </summary>
    Protected Overrides Sub OnTextBoxResize(ByVal source As Object, ByVal e As System.EventArgs)
        If _textbox Is Nothing Then Exit Sub
        If _showUpDownButtons = ShowUpDownButtonsMode.Always Then
            ' standard management
            MyBase.OnTextBoxResize(source, e)
        Else
            ' custom management

            ' change position if RTL
            Dim fixPos As Boolean = Me.RightToLeft = Windows.Forms.RightToLeft.Yes _
                                Xor Me.UpDownAlign = LeftRightAlignment.Left

            If _mouseOver Then
                _textbox.Width = Me.ClientSize.Width - _textbox.Left - _upDownButtons.Width - 2
                If fixPos Then _textbox.Location = New Point(16, _textbox.Location.Y)
            Else
                If fixPos Then _textbox.Location = New Point(2, _textbox.Location.Y)
                _textbox.Width = Me.ClientSize.Width - _textbox.Left - 2
            End If

        End If

    End Sub

#End Region


End Class

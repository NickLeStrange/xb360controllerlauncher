Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Input
Imports Microsoft.Win32
Imports SharpDX
Imports SharpDX.DirectInput
Imports System.Text.RegularExpressions

Public Class frmMain

#Region "DECLARES"

    'Set Steam and Kodi as processes, and create Booleans for them to be used.
    Dim Steam() As Process, Kodi() As Process
    Dim SteamIsRunning As Boolean, KodiIsRunning As Boolean

    'This integer helps to keep track of the currently focused button for navigation.
    Dim CurrentButton As Integer

    'Xbox Controller input-related globals
    Dim ControllerConnected As Boolean, StartPressed As Boolean,
        DPadLeft As Boolean, DPadRight As Boolean, ButtonAPressed As Boolean

    'DS4 Controller input-related globals
    Dim gotDS4Object As Boolean, DS4IsConnected As Boolean, ButtonXPressed As Boolean,
        DS4DpadLeft As Boolean, DS4DpadRight As Boolean, OrbPressed As Boolean
    Dim ds4Gamepad As Object, ds4State As Object

#End Region

#Region "CHECK IF RUNNING"

    Private Sub CheckIfRunning()
        'Check if either of the processes is running.
        'This will be used to make sure that when the start button on the controller is 
        'pressed that it won't interfere with your game.
        Steam = Process.GetProcessesByName("Steam")
        Kodi = Process.GetProcessesByName("Kodi")

        If Steam.Count > 0 Then
            'If Steam is running then set the Boolean to True
            SteamIsRunning = True
        Else
            'If not, set the Boolean to False.
            SteamIsRunning = False
        End If

        If Kodi.Count > 0 Then
            'If Kodi is running then set the Boolean to True
            KodiIsRunning = True
        Else
            'If not, set the Boolean to False.
            KodiIsRunning = False
        End If
    End Sub

#End Region

#Region "FORM CONTROL"

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Load up whatever needs to be loaded.
        GamepadDetect()
        FindPaths()
        ButtonBorders()
    End Sub

    Private Sub frmMain_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        btnSteam.Focus()
        Me.Visible = False
    End Sub

#End Region

#Region "FORM FADES"

    Public Sub FadeFormIn()

        'This fades the form in when it is shown on the screen.
        For FadeIn = 0.0 To 1.1 Step 0.1
            Me.Opacity = FadeIn
            Me.Refresh()
            Threading.Thread.Sleep(20)
        Next
    End Sub

    Public Sub FadeFormOut()

        'This fades the form out when called after exiting this application or after selecting an application to start.
        For FadeOut = 1.1 To 0.0 Step -0.1
            Me.Opacity = FadeOut
            Me.Refresh()
            Threading.Thread.Sleep(20)
        Next
    End Sub

#End Region

#Region "NOTIFYICON"

    Private Sub NotifyIcon_MouseClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon.MouseClick
        'If the notify icon is right clicked, show the toolstripmenu named NotifyIconMenu at the position of the click.
        If e.Button = MouseButtons.Right Then
            NotifyIconMenu.Show(Cursor.Position)
        End If
    End Sub

    Private Sub NotifyIconMenuExit_Click(sender As Object, e As EventArgs) Handles NotifyIconMenuExit.Click
        'When the menu item Exit is clicked, exit the application.
        Application.Exit()
    End Sub

#End Region

#Region "APPLICATION PATHS"

    Public Sub FindPaths()
        Dim tempKodiPath As String
        Dim tempSteamPath As String

        'Get Steam's full path, including it's exe name, from it's registry keys 
        tempSteamPath = Registry.GetValue("HKEY_CURRENT_USER\SOFTWARE\Valve\Steam", "SteamExe", "Key does not exist")
        'If the key exists, store the exe path in application settings
        If tempSteamPath <> "Key does not exist" Then
            My.Settings.savedSteamPath = tempSteamPath
        Else
            MessageBox.Show("Cannot find Steam.exe using the path in the registry", "Error")
        End If

        'Kodi stores folder location in a (Default) reg key. First test for Kodi's subkey
        'before attempting to get value from (Default) key. This way avoids getting a 
        'bogus value if Kodi is uninstalled.
        Dim regTestForSubkey As RegistryKey
        regTestForSubkey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Kodi", True)
        tempKodiPath = Registry.GetValue("HKEY_CURRENT_USER\SOFTWARE\Kodi", "", "Key does not exist")
        'If the Kodi subkey exists AND the (Default) key is returning values, append the kodi.exe 
        'file name and save in application settings.
        If tempKodiPath <> "Key does not exist" Or regTestForSubkey IsNot Nothing Then
            tempKodiPath += "\kodi.exe"
            My.Settings.savedKodiPath = tempKodiPath
        Else
            MessageBox.Show("Cannot find Kodi.exe path in registry", "Error")
        End If
    End Sub

#End Region

#Region "BUTTONS"

    Public Sub ButtonBorders()
        'Manage the borders of the buttons on load.
        btnExit.FlatAppearance.BorderSize = 0
        btnKodi.FlatAppearance.BorderSize = 0
        btnSteam.FlatAppearance.BorderSize = 0
    End Sub

    Private Sub btnSteam_Click(sender As Object, e As EventArgs) Handles btnSteam.Click
        'Start Steam using the path saved in application's settings. This command won't 
        'actually force it into big picture if it Is already running. You have to configure 
        'Steam to start in Big Picture Mode when it Is launched. You always have to exit 
        'Steam completely.
        Process.Start(My.Settings.savedSteamPath, "-bigpicture")
        FadeFormOut()
        Me.Visible = False
        NotifyIcon.Visible = True
    End Sub

    Private Sub btnSteam_GotFocus(sender As Object, e As EventArgs) Handles btnSteam.GotFocus
        'If the Steam button gets focus, set the other buttons accordingly.
        CurrentButton = 1
        btnSteam.FlatAppearance.BorderSize = 10
        btnKodi.FlatAppearance.BorderSize = 0
        btnExit.FlatAppearance.BorderSize = 0
        lblLaunchWhat.Text = "Press A to launch STEAM"
        btnSteam.Image = LaunchButtons.Images.Item(3)
        btnKodi.Image = LaunchButtons.Images.Item(0)
    End Sub

    Private Sub btnKodi_Click(sender As Object, e As EventArgs) Handles btnKodi.Click
        'Start Kodi using the path saved in application's settings
        Process.Start(My.Settings.savedKodiPath)
        FadeFormOut()
        Me.Visible = False
        NotifyIcon.Visible = True
    End Sub

    Private Sub btnKodi_GotFocus(sender As Object, e As EventArgs) Handles btnKodi.GotFocus
        'If the Kodi button gets focus, set the other buttons accordingly.
        CurrentButton = 2
        btnKodi.FlatAppearance.BorderSize = 10
        btnSteam.FlatAppearance.BorderSize = 0
        btnExit.FlatAppearance.BorderSize = 0
        lblLaunchWhat.Text = "Press A to launch KODI"
        btnSteam.Image = LaunchButtons.Images.Item(2)
        btnKodi.Image = LaunchButtons.Images.Item(1)
    End Sub


    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        'Exit
        FadeFormOut()
        Me.Close()
    End Sub

    Private Sub btnExit_GotFocus(sender As Object, e As EventArgs) Handles btnExit.GotFocus
        'If the Exit button gets focus, set the other buttons accordingly.
        CurrentButton = 3
        btnExit.FlatAppearance.BorderSize = 5
        btnSteam.FlatAppearance.BorderSize = 0
        btnKodi.FlatAppearance.BorderSize = 0
        lblLaunchWhat.Text = "Press A to QUIT"
        btnKodi.Image = LaunchButtons.Images.Item(0)
        btnSteam.Image = LaunchButtons.Images.Item(2)
    End Sub

#End Region

#Region "GAMEPAD DETECTION"

    Private Sub GamepadDetect()
        'Initialise DirectInput.
        Dim DirectInput = New DirectInput

        'Find the first gamepad's name. Loop until a device instance is found.
        Dim deviceInstance As DeviceInstance = Nothing
        Do While deviceInstance Is Nothing
            Try
                deviceInstance = DirectInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices)(0)
            Catch ex As Exception
            End Try
        Loop
        Dim gamepadName = deviceInstance.ProductName

        'Clear string of unwanted characters.
        gamepadName = Regex.Replace(gamepadName, "[^A-Za-z0-9\-/ ]", "")

        'Start appropriate timer depending on gamepad name found.
        If gamepadName.IndexOf("XBOX", StringComparison.CurrentCultureIgnoreCase) > -1 Then
            tmrControllerDetect.Start()
        ElseIf gamepadName = "Wireless Controller" Then
            tmrDS4Detect.Start()
        End If
    End Sub

#End Region

#Region "HANDLE XBOX CONTROLLER INPUT"

    Private Sub tmrControllerDetect_Tick(sender As Object, e As EventArgs) Handles tmrControllerDetect.Tick
        'Set up the xbox 360 controller buttons.
        Dim currentState As GamePadState = GamePad.GetState(PlayerIndex.One)

        'Check if Steam or Kodi is running.
        CheckIfRunning()

        If SteamIsRunning = False And KodiIsRunning = False Then
            'Neither Steam nor Kodi is running, so make use of the buttons.
            If currentState.IsConnected Then
                If ControllerConnected = False Then
                    ControllerConnected = True
                    Me.Visible = True
                    FadeFormIn()
                End If

                'Use the A button on the controller to launch the apps.
                If currentState.Buttons.A = ButtonState.Pressed Then
                    lblLaunchWhat.Text = "LAUNCHING SELECTION"
                    ButtonAPressed = True
                ElseIf currentState.Buttons.A = ButtonState.Released Then
                    If ButtonAPressed = True Then
                        'Simulate the pressing of the Enter key to press the button.
                        SendKeys.Send("{ENTER}")
                        ButtonAPressed = False
                    End If
                End If

                'Use the DPAD to go left.
                If currentState.DPad.Left = ButtonState.Pressed Then
                    DPadLeft = True
                ElseIf currentState.DPad.Left = ButtonState.Released Then
                    If DPadLeft = True Then
                        If CurrentButton = 1 Then
                            btnExit.Focus()
                            DPadLeft = False
                        ElseIf CurrentButton = 2 Then
                            btnSteam.Focus()
                            DPadLeft = False
                        ElseIf CurrentButton = 3 Then
                            btnKodi.Focus()
                            DPadLeft = False
                        End If
                    End If
                End If

                'Use the DPAD to go Right.
                If currentState.DPad.Right = ButtonState.Pressed Then
                    DPadRight = True
                ElseIf currentState.DPad.Right = ButtonState.Released Then
                    If DPadRight = True Then
                        If CurrentButton = 1 Then
                            btnKodi.Focus()
                            DPadRight = False
                        ElseIf CurrentButton = 2 Then
                            btnExit.Focus()
                            DPadRight = False
                        ElseIf CurrentButton = 3 Then
                            btnSteam.Focus()
                            DPadRight = False
                        End If
                    End If
                End If

                'Use the Start button to show the launcher.
                If currentState.Buttons.Start = ButtonState.Pressed Then
                    StartPressed = True
                ElseIf currentState.Buttons.Start = ButtonState.Released Then
                    If StartPressed = True Then
                        If Me.Visible = True Then
                            Me.Visible = False
                            NotifyIcon.Visible = True
                            StartPressed = False
                        ElseIf Me.Visible = False Then
                            Me.Visible = True
                            FadeFormIn()
                            NotifyIcon.Visible = False
                            StartPressed = False
                        End If
                    End If
                End If

                'Use the left analogue stick to select between apps.
                If currentState.ThumbSticks.Left.X = 1 Then

                    'When pressing left on the analogue stick it cycles through the buttons to the left.
                    If CurrentButton = 1 Then
                        btnKodi.Focus()

                    ElseIf CurrentButton = 2 Then
                        btnExit.Focus()

                    ElseIf CurrentButton = 3 Then
                        btnSteam.Focus()

                    End If
                ElseIf currentState.ThumbSticks.Left.X = -1 Then
                    'When pressing left on the analogue stick it cycles through the buttons to the right.
                    If CurrentButton = 1 Then
                        btnExit.Focus()

                    ElseIf CurrentButton = 2 Then
                        btnSteam.Focus()

                    ElseIf CurrentButton = 3 Then
                        btnKodi.Focus()

                    End If
                End If

            Else
                ControllerConnected = False
                Me.Visible = False
            End If
        End If
    End Sub

#End Region

#Region "HANDLE DS4 CONTROLLER INPUT"

    Private Function DS4Init() As Object
        'Initialise DirectInput.
        Dim directInput As New DirectInput
        'Find the DS4's Guid.
        Dim controllerGuid = Guid.Empty

        For Each deviceInstance In directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices)
            controllerGuid = deviceInstance.InstanceGuid
        Next

        'Instantiate the dsGamepad object.
        Dim ds4Gamepad As New Joystick(directInput, controllerGuid)
        'Acquire the DS4.
        ds4Gamepad.Acquire()
        'A DS4 controller is connected if this point is reached.
        DS4IsConnected = True
        'Return the ds4Gamepad object to the calling sub, tmrDS4Detect timer 
        Return ds4Gamepad
    End Function

    Private Sub tmrDS4Detect_Tick(sender As Object, e As EventArgs) Handles tmrDS4Detect.Tick
        'If this is the first tick then the ds4Gamepad object must be returned from the DS4Init function.
        If gotDS4Object = False Then
            ds4Gamepad = DS4Init()
            gotDS4Object = True
        End If

        'Update the global ds4State object with the controller's state on every timer tick
        Try
            ds4State = ds4Gamepad.GetCurrentState
        Catch ex As Exception
            'Controller is lost. Reset init bool, stop timer, show balloon, call GamepadDetect().
            gotDS4Object = False
            tmrDS4Detect.Stop()
            NotifyIcon.BalloonTipText = "Controller disconnected"
            NotifyIcon.ShowBalloonTip(2000)
            GamepadDetect()
        End Try

        'Use the X button on the controller to launch the apps.
        If ds4State.Buttons(1) = True Then
            lblLaunchWhat.Text = "LAUNCHING SELECTION"
            ButtonXPressed = True
        ElseIf ds4State.Buttons(1) = False Then
            If ButtonXPressed = True Then
                'Simulate the pressing of the Enter key to press the button.
                SendKeys.Send("{ENTER}")
                ButtonXPressed = False
            End If
        End If

        'Use the DPAD to go left.
        If ds4State.PointOfViewControllers(0) = 27000 Then
            DS4DpadLeft = True
        ElseIf ds4State.PointOfViewControllers(0) = -1 Then
            If DS4DpadLeft = True Then
                If CurrentButton = 1 Then
                    btnExit.Focus()
                    DS4DpadLeft = False
                ElseIf CurrentButton = 2 Then
                    btnSteam.Focus()
                    DS4DpadLeft = False
                ElseIf CurrentButton = 3 Then
                    btnKodi.Focus()
                    DS4DpadLeft = False
                End If
            End If
        End If

        'Use the DPAD to go Right.
        If ds4State.PointOfViewControllers(0) = 9000 Then
            DS4DpadRight = True
        ElseIf ds4State.PointOfViewControllers(0) = -1 Then
            If DS4DpadRight = True Then
                If CurrentButton = 1 Then
                    btnKodi.Focus()
                    DS4DpadRight = False
                ElseIf CurrentButton = 2 Then
                    btnExit.Focus()
                    DS4DpadRight = False
                ElseIf CurrentButton = 3 Then
                    btnSteam.Focus()
                    DS4DpadRight = False
                End If
            End If
        End If


        'Use the PS Orb button to show the launcher.
        If ds4State.Buttons(12) = True Then
            OrbPressed = True
        ElseIf ds4State.Buttons(12) = False Then
            If OrbPressed = True Then
                If Me.Visible = True Then
                    Me.Visible = False
                    NotifyIcon.Visible = True
                    OrbPressed = False
                ElseIf Me.Visible = False Then
                    Me.Visible = True
                    FadeFormIn()
                    NotifyIcon.Visible = False
                    OrbPressed = False
                End If
            End If
        End If

        'Use the left analogue stick to select between apps.
        If ds4State.x > 35000 Then
            'When pressing left on the analogue stick it cycles through the buttons to the left.
            If CurrentButton = 1 Then
                btnKodi.Focus()

            ElseIf CurrentButton = 2 Then
                btnExit.Focus()

            ElseIf CurrentButton = 3 Then
                btnSteam.Focus()
            End If
        ElseIf ds4State.x < 20000 Then
            'When pressing left on the analogue stick it cycles through the buttons to the right.
            If CurrentButton = 1 Then
                btnExit.Focus()

            ElseIf CurrentButton = 2 Then
                btnSteam.Focus()

            ElseIf CurrentButton = 3 Then
                btnKodi.Focus()
            End If
        End If

        'For future reference. The values below are what is retrieved when a buffer is set up and getBufferedData() is used. 
        'In which case 128 is the value when button is depressed, -1 for when button is released, and 0 at all other times. 
        'When getCurrentState() is used, as above, then all buttons constantly fire False, and only fire True while depressed.
        'Square             - Buttons(0), value: 128
        'X                  - Buttons(1), value: 128
        'Circle             - Buttons(2), value: 128
        'Triangle           - Buttons(3), value: 128
        'L1                 - Buttons(4), value: 128
        'R2                 - Buttons(5), value: 128
        'Share              - Buttons(8), value: 128
        'Options            - Buttons(9), value: 128
        'Left Stick Click   - Buttons(10), value: 128
        'Right Stick Click  - Buttons(11), value: 128
        'PS Orb             - Buttons(12), value: 128
        'Touchpad Click     - Buttons(13), value: 128

        'Valid for getBufferedData() and getCurrentState()
        'L2                 - Buttons(6), value: 0 to 65535
        'R2                 - Buttons(7), value: 0 to 65535

        'Valid for getBufferedData() and getCurrentState()
        'Dpad Left          - PointofViewControllers(0), value: 27000
        'Dpad Down          - PointofViewControllers(0), value: 18000
        'Dpad Right         - PointofViewControllers(0), value: 9000
        'Dpad Up            - PointofViewControllers(0), value: 0
        'Dpad Up-Left       - PointofViewControllers(0), value: 31500
        'Dpad Up-Right      - PointofViewControllers(0), value: 4500
        'Dpad Down-Left     - PointofViewControllers(0), value: 22500
        'Dpad Down-Right    - PointofViewControllers(0), value: 13500

        ' Stick co-ords.
        ' (0,0) (32768,0) (65535, 0) 
        ' (0, 32768) (32768, 32768) (65535, 32768)
        ' (0, 65535) (32768, 65535) (65535, 65535)
    End Sub
#End Region
End Class

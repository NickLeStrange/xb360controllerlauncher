Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Input
Imports Microsoft.Win32

Public Class frmMain

#Region "DECLARES"

    'Set Steam and Kodi as processes, and create Booleans for them to be used.
    Dim Steam() As Process, Kodi() As Process
    Dim SteamIsRunning As Boolean, KodiIsRunning As Boolean, ControllerConnected As Boolean, StartPressed As Boolean, _
        DPadLeft As Boolean, DPadRight As Boolean, ButtonAPressed As Boolean
    'This integer helps to keep track of the currently focused button for navigation.
    Dim CurrentButton As Integer

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
        ButtonBorders()
        FindPaths()
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

        'Kodi stores it's exe's folder location in a (Default) reg key, which always returns a value even when it does not exist. So first test for Kodi's subkey
        'before attempting to get the value from (Default) key. This way avoids getting a bogus value if Kodi is not installed.
        Dim regTestForSubkey As RegistryKey
        regTestForSubkey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Kodi", True)
        tempKodiPath = Registry.GetValue("HKEY_CURRENT_USER\SOFTWARE\Kodi", "", "Key does not exist")
        'If the Kodi subkey exists AND the (Default) key is returning values, append the kodi.exe file name and save in application settings.
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

#Region "TIMERS"

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

End Class

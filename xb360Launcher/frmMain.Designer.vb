<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.btnKodi = New System.Windows.Forms.Button()
        Me.LaunchButtons = New System.Windows.Forms.ImageList(Me.components)
        Me.btnSteam = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.lblLaunchWhat = New System.Windows.Forms.Label()
        Me.tmrControllerDetect = New System.Windows.Forms.Timer(Me.components)
        Me.pnlBackground = New System.Windows.Forms.Panel()
        Me.NotifyIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.NotifyIconMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NotifyIconMenuExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.tmrDS4Detect = New System.Windows.Forms.Timer(Me.components)
        Me.pnlBackground.SuspendLayout()
        Me.NotifyIconMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnKodi
        '
        Me.btnKodi.BackColor = System.Drawing.Color.Transparent
        Me.btnKodi.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue
        Me.btnKodi.FlatAppearance.BorderSize = 10
        Me.btnKodi.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnKodi.ImageIndex = 0
        Me.btnKodi.ImageList = Me.LaunchButtons
        Me.btnKodi.Location = New System.Drawing.Point(300, 9)
        Me.btnKodi.Name = "btnKodi"
        Me.btnKodi.Size = New System.Drawing.Size(169, 168)
        Me.btnKodi.TabIndex = 2
        Me.btnKodi.UseVisualStyleBackColor = False
        '
        'LaunchButtons
        '
        Me.LaunchButtons.ImageStream = CType(resources.GetObject("LaunchButtons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.LaunchButtons.TransparentColor = System.Drawing.Color.Transparent
        Me.LaunchButtons.Images.SetKeyName(0, "Kodi_Button.png")
        Me.LaunchButtons.Images.SetKeyName(1, "Kodi_Button_Highlight.png")
        Me.LaunchButtons.Images.SetKeyName(2, "Steam_Button.png")
        Me.LaunchButtons.Images.SetKeyName(3, "Steam_Button_Highlight.png")
        '
        'btnSteam
        '
        Me.btnSteam.BackColor = System.Drawing.Color.Transparent
        Me.btnSteam.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue
        Me.btnSteam.FlatAppearance.BorderSize = 10
        Me.btnSteam.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnSteam.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSteam.ImageIndex = 2
        Me.btnSteam.ImageList = Me.LaunchButtons
        Me.btnSteam.Location = New System.Drawing.Point(67, 9)
        Me.btnSteam.Name = "btnSteam"
        Me.btnSteam.Size = New System.Drawing.Size(169, 168)
        Me.btnSteam.TabIndex = 1
        Me.btnSteam.UseVisualStyleBackColor = False
        '
        'btnExit
        '
        Me.btnExit.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue
        Me.btnExit.FlatAppearance.BorderSize = 5
        Me.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExit.Location = New System.Drawing.Point(414, 12)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(106, 49)
        Me.btnExit.TabIndex = 4
        Me.btnExit.Text = "Exit"
        Me.btnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'lblLaunchWhat
        '
        Me.lblLaunchWhat.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLaunchWhat.Location = New System.Drawing.Point(12, 274)
        Me.lblLaunchWhat.Name = "lblLaunchWhat"
        Me.lblLaunchWhat.Size = New System.Drawing.Size(508, 36)
        Me.lblLaunchWhat.TabIndex = 0
        Me.lblLaunchWhat.Text = "..."
        Me.lblLaunchWhat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tmrControllerDetect
        '
        '
        'pnlBackground
        '
        Me.pnlBackground.BackColor = System.Drawing.Color.DimGray
        Me.pnlBackground.Controls.Add(Me.btnSteam)
        Me.pnlBackground.Controls.Add(Me.btnKodi)
        Me.pnlBackground.Location = New System.Drawing.Point(0, 76)
        Me.pnlBackground.Name = "pnlBackground"
        Me.pnlBackground.Size = New System.Drawing.Size(534, 187)
        Me.pnlBackground.TabIndex = 5
        '
        'NotifyIcon
        '
        Me.NotifyIcon.Icon = CType(resources.GetObject("NotifyIcon.Icon"), System.Drawing.Icon)
        Me.NotifyIcon.Text = "Xb360 Launcher"
        Me.NotifyIcon.Visible = True
        '
        'NotifyIconMenu
        '
        Me.NotifyIconMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NotifyIconMenuExit})
        Me.NotifyIconMenu.Name = "NotifyIconMenu"
        Me.NotifyIconMenu.Size = New System.Drawing.Size(93, 26)
        '
        'NotifyIconMenuExit
        '
        Me.NotifyIconMenuExit.Name = "NotifyIconMenuExit"
        Me.NotifyIconMenuExit.Size = New System.Drawing.Size(92, 22)
        Me.NotifyIconMenuExit.Text = "Exit"
        '
        'tmrDS4Detect
        '
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightGray
        Me.ClientSize = New System.Drawing.Size(532, 324)
        Me.Controls.Add(Me.lblLaunchWhat)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.pnlBackground)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmMain"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.pnlBackground.ResumeLayout(False)
        Me.NotifyIconMenu.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnKodi As System.Windows.Forms.Button
    Friend WithEvents btnSteam As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents lblLaunchWhat As System.Windows.Forms.Label
    Friend WithEvents LaunchButtons As System.Windows.Forms.ImageList
    Friend WithEvents tmrControllerDetect As System.Windows.Forms.Timer
    Friend WithEvents pnlBackground As System.Windows.Forms.Panel
    Friend WithEvents NotifyIcon As System.Windows.Forms.NotifyIcon
    Friend WithEvents NotifyIconMenu As ContextMenuStrip
    Friend WithEvents NotifyIconMenuExit As ToolStripMenuItem
    Friend WithEvents tmrDS4Detect As Timer
End Class

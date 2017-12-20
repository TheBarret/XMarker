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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.txtMessage = New System.Windows.Forms.TextBox()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.btnResolve = New System.Windows.Forms.Button()
        Me.Viewport = New GUI.Viewport()
        Me.SuspendLayout()
        '
        'txtMessage
        '
        Me.txtMessage.AcceptsReturn = True
        Me.txtMessage.AcceptsTab = True
        Me.txtMessage.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMessage.Location = New System.Drawing.Point(146, 12)
        Me.txtMessage.Multiline = True
        Me.txtMessage.Name = "txtMessage"
        Me.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtMessage.Size = New System.Drawing.Size(335, 128)
        Me.txtMessage.TabIndex = 1
        Me.txtMessage.Text = "1234567890 !@#$%^&*()" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "abcdefghijklmnopqrstuvwxyz"
        '
        'btnGenerate
        '
        Me.btnGenerate.Location = New System.Drawing.Point(385, 146)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(96, 37)
        Me.btnGenerate.TabIndex = 2
        Me.btnGenerate.Text = "Generate"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'btnResolve
        '
        Me.btnResolve.Location = New System.Drawing.Point(12, 146)
        Me.btnResolve.Name = "btnResolve"
        Me.btnResolve.Size = New System.Drawing.Size(128, 37)
        Me.btnResolve.TabIndex = 4
        Me.btnResolve.Text = "Resolve"
        Me.btnResolve.UseVisualStyleBackColor = True
        '
        'Viewport
        '
        Me.Viewport.BackColor = System.Drawing.Color.White
        Me.Viewport.BackgroundImage = CType(resources.GetObject("Viewport.BackgroundImage"), System.Drawing.Image)
        Me.Viewport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.Viewport.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Viewport.Location = New System.Drawing.Point(12, 12)
        Me.Viewport.Name = "Viewport"
        Me.Viewport.Size = New System.Drawing.Size(128, 128)
        Me.Viewport.TabIndex = 0
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(497, 198)
        Me.Controls.Add(Me.btnResolve)
        Me.Controls.Add(Me.btnGenerate)
        Me.Controls.Add(Me.txtMessage)
        Me.Controls.Add(Me.Viewport)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "XMarker"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Viewport As GUI.Viewport
    Friend WithEvents txtMessage As System.Windows.Forms.TextBox
    Friend WithEvents btnGenerate As System.Windows.Forms.Button
    Friend WithEvents btnResolve As System.Windows.Forms.Button

End Class

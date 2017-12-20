Imports XMarker

Public Class frmMain

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        If (Not String.IsNullOrEmpty(Me.txtMessage.Text)) Then
            Me.Viewport.BackgroundImage = New Generator().Embed(My.Resources.image, Me.txtMessage.Text)
            Me.txtMessage.Clear()
        End If
    End Sub
    Private Sub btnResolve_Click(sender As Object, e As EventArgs) Handles btnResolve.Click
        If (Me.Viewport.BackgroundImage IsNot Nothing) Then
            Me.txtMessage.Clear()
            Me.txtMessage.Text = New Generator().Resolve(CType(Me.Viewport.BackgroundImage, Bitmap))
        End If
    End Sub
End Class

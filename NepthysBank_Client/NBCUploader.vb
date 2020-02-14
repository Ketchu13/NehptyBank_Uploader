Imports System.Net
Imports System.Text
Imports System.IO

'**NBC0.1 by K13/Søuris KFP - for Nephtys Auberdine EU

Public Class NBCUploader
    Private isMouseDown As Boolean = False
    Private mouseOffset As Point
    Private file_content As String
    Private server_ip As String = "91.164.99.220:8888"

#Region "Drag and Move form"
    ' Left mouse button pressed
    Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            ' Get the new position
            mouseOffset = New Point(-e.X, -e.Y)
            ' Set that left button is pressed
            isMouseDown = True
        End If
    End Sub

    ' MouseMove used to check if mouse cursor is moving
    Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        If isMouseDown Then
            Dim mousePos As Point = Control.MousePosition
            ' Get the new form position
            mousePos.Offset(mouseOffset.X, mouseOffset.Y)
            Me.Location = mousePos
        End If
    End Sub

    ' Left mouse button released, form should stop moving
    Private Sub Form1_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Left Then
            isMouseDown = False
        End If
    End Sub
#End Region

#Region "UI controls"
    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Dim rslt As DialogResult = OpenFileDialog1.ShowDialog(Me)
        If rslt = DialogResult.OK Then
            Dim file_name As String = OpenFileDialog1.FileName
            If File.Exists(file_name) Then
                file_content = File.ReadAllText(file_name)
                Label1.Text = file_name.Split("\").Reverse()(0)
                Label3.Text = "Le fichier est prêt à être envoyé.."
            Else
                Label3.Text = "Erreur lors de la lecture du fichier..."
            End If
        End If

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Len(file_content) > 10 Then
            Dim postData As String = "cnt=" & Uri.EscapeDataString(file_content)
            Dim tempCookies As New CookieContainer
            Dim encoding As New UTF8Encoding
            Dim byteData As Byte() = encoding.GetBytes(postData)

            Dim postReq As HttpWebRequest = DirectCast(WebRequest.Create("http://" & server_ip & "/cgi-bin/Index.py"), HttpWebRequest)
            postReq.Method = "POST"
            postReq.KeepAlive = True
            postReq.CookieContainer = tempCookies
            postReq.ContentType = "application/x-www-form-urlencoded"
            postReq.Referer = "http://91.164.99.220:8888/"
            postReq.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; ru; rv:1.9.2.3) Gecko/20100401 Firefox/4.0 (.NET CLR 3.5.30729)"
            postReq.ContentLength = byteData.Length

            Dim postreqstream As Stream = postReq.GetRequestStream()
            postreqstream.Write(byteData, 0, byteData.Length)
            postreqstream.Close()
        Else
            Label3.Text = "Le fichier semble corrompu ou ne pas contenir les données d'une banque de guilde..."
        End If
        Label1.Text = "Aucun fichier choisi"
        Label3.Text = "Les données ont été envoyé à BabyBot.. Merci !"


    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        End
    End Sub
#End Region

#Region "UI Events"
    Private Sub Label1_TextChanged(sender As Object, e As EventArgs) Handles Label1.TextChanged
        If Label1.Text = "BagBrother.lua" Then
            Button3.Enabled = True
        Else
            Button3.Enabled = False
        End If
    End Sub
#End Region

End Class

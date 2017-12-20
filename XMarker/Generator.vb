Imports System.Text
Imports System.Drawing

Public Class Generator
    Public Function Embed(bm As Bitmap, Message As String) As Bitmap
        Return Generator.Write(bm, Message)
    End Function
    Public Function Resolve(bm As Bitmap) As String
        Dim collection As List(Of Integer) = Generator.Read(bm)
        Dim index As Integer = 0, buffer As New StringBuilder, value As String
        Do
            value = Generator.Consume(collection, 8)
            If (Strings.AscW(value) >= 32 AndAlso Strings.AscW(value) <= 126 Or value = Chr(10) Or value = Chr(13)) Then
                buffer.Append(value)
                index += 7
                Continue Do
            End If
            Exit Do
        Loop Until index >= collection.Count - 1
        Return buffer.ToString
    End Function
    Private Shared Function Write(bm As Bitmap, message As String) As Bitmap
        Dim length As Integer = 0, offset As Integer = 0, dst As IEnumerable(Of Bit) = Nothing, src As IEnumerable(Of Bit) = Generator.ToBits(message)
        If (bm.Width * bm.Height * [Enum].GetValues(GetType(Position)).Length * [Enum].GetValues(GetType(Channel)).Length >= src.Count) Then
            For Each p As Position In [Enum].GetValues(GetType(Position))
                For Each c As Channel In [Enum].GetValues(GetType(Channel))
                    For y As Integer = 0 To bm.Height - 1
                        length = Generator.Read(src, dst, offset, bm.Width)
                        If (length > 0) Then
                            For x As Integer = 0 To dst.Count - 1
                                Generator.Write(bm, x, y, Channel.R, p, dst.ElementAt(x).Value)
                            Next
                            offset += length
                            length = 0
                        ElseIf (length = 0) Then
                            Return bm
                        End If
                    Next
                Next
            Next
        End If
        Throw New Exception("message too long for this image to store")
    End Function
    Private Shared Sub Write(bm As Bitmap, x As Integer, y As Integer, c As Channel, p As Position, bit As BitValue)
        Dim result As IEnumerable(Of Bit) = Nothing, pixel As Color = bm.GetPixel(x, y)
        Select Case c
            Case Channel.R
                result = Generator.ToBits(pixel.R)
                result.ElementAt(p).Change(bit)
                bm.SetPixel(x, y, Color.FromArgb(Generator.ToByte(result), pixel.G, pixel.B))
            Case Channel.G
                result = Generator.ToBits(pixel.G)
                result.ElementAt(p).Change(bit)
                bm.SetPixel(x, y, Color.FromArgb(pixel.R, Generator.ToByte(result), pixel.B))
            Case Channel.B
                result = Generator.ToBits(pixel.B)
                result.ElementAt(p).Change(bit)
                bm.SetPixel(x, y, Color.FromArgb(pixel.R, pixel.G, Generator.ToByte(result)))
        End Select
    End Sub
    Private Shared Function Read(src As IEnumerable(Of Bit), ByRef dst As IEnumerable(Of Bit), offset As Integer, len As Integer) As Integer
        Dim result As New List(Of Bit)
        If (offset >= 0 AndAlso (offset + len - 1) <= src.Count - 1) Then
            For i As Integer = offset To offset + len - 1
                result.Add(src.ElementAt(i))
            Next
        ElseIf (offset + (len - 1) >= src.Count - 1) Then
            For i As Integer = offset To src.Count - 1
                result.Add(src.ElementAt(i))
            Next
        End If
        dst = result
        Return result.Count
    End Function
    Private Shared Function Read(src As IEnumerable(Of Color), ByRef dst As IEnumerable(Of Color), offset As Integer, len As Integer) As Integer
        Dim result As New List(Of Color)
        If (offset >= 0 AndAlso (offset + len - 1) <= src.Count - 1) Then
            For i As Integer = offset To offset + len - 1
                result.Add(src.ElementAt(i))
            Next
        ElseIf (offset + (len - 1) >= src.Count - 1) Then
            For i As Integer = offset To src.Count - 1
                result.Add(src.ElementAt(i))
            Next
        End If
        dst = result
        Return result.Count
    End Function
    Private Shared Function Read(bm As Bitmap) As IEnumerable(Of Integer)
        Dim collection As New List(Of Integer)
        For Each p As Position In [Enum].GetValues(GetType(Position))
            For Each c As Channel In [Enum].GetValues(GetType(Channel))
                For y As Integer = 0 To bm.Height - 1
                    For x As Integer = 0 To bm.Width - 1
                        If (Generator.Read(bm, p, c, x, y).Value = BitValue.Zero) Then
                            collection.Add(0)
                        Else
                            collection.Add(1)
                        End If
                    Next
                Next
            Next
        Next
        Return collection
    End Function
    Private Shared Function Read(bm As Bitmap, p As Position, c As Channel, x As Integer, y As Integer) As Bit
        Dim current As Color = bm.GetPixel(x, y), value As Bit = Nothing
        Select Case c
            Case Channel.R : value = Generator.ToBits(current.R).ElementAt(p)
            Case Channel.G : value = Generator.ToBits(current.G).ElementAt(p)
            Case Channel.B : value = Generator.ToBits(current.B).ElementAt(p)
        End Select
        Return value
    End Function
    Private Shared Function Consume(collection As List(Of Integer), length As Integer) As String
        Dim value As String = String.Empty
        If (length < collection.Count - length) Then
            value = Strings.ChrW(Convert.ToByte(String.Concat(collection.Take(length)), 2))
            collection.RemoveRange(0, length)
        End If
        Return value
    End Function
    Private Shared Function ToBits(Value As Byte) As IEnumerable(Of Bit)
        Dim buffer As New List(Of Bit)
        For Each x As Char In Convert.ToString(Value, 2).PadLeft(8, "0"c).ToCharArray
            If (x = "0"c) Then
                buffer.Add(New Bit(BitValue.Zero))
            Else
                buffer.Add(New Bit(BitValue.One))
            End If
        Next
        Return buffer
    End Function
    Private Shared Function ToBits(Value As String) As IEnumerable(Of Bit)
        Dim buffer As New List(Of Bit)
        For Each x As Char In Value.ToCharArray
            For Each y As Char In Convert.ToString(Strings.AscW(x), 2).PadLeft(8, "0"c).ToCharArray
                If (y = "0"c) Then
                    buffer.Add(New Bit(BitValue.Zero))
                Else
                    buffer.Add(New Bit(BitValue.One))
                End If
            Next
        Next
        Return buffer
    End Function
    Private Shared Function ToByte(Stream As IEnumerable(Of Bit)) As Byte
        Dim value As String = String.Empty
        For Each Bit As Bit In Stream
            If (Bit.Value = BitValue.Zero) Then
                value &= 0
            Else
                value &= 1
            End If
        Next
        Return Convert.ToByte(value, 2)
    End Function
End Class

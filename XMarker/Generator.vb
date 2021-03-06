﻿Imports System.Text
Imports System.Drawing

Public Class Generator
#Region "Public"
    Public Function Embed(bm As Bitmap, Message As String) As Bitmap
        Return Generator.Write(bm, Message)
    End Function
    Public Function Resolve(bm As Bitmap) As String
        Dim src As List(Of Byte) = Generator.Read(bm)
        Dim index As Integer = 0, buffer As New StringBuilder, value As String
        Do
            value = Generator.Consume(src, Bit.Length)
            If (Generator.IsReadable(value)) Then
                buffer.Append(value)
                index += Bit.Length - 1
                Continue Do
            End If
            Exit Do
        Loop Until index >= src.Count - 1
        Return buffer.ToString
    End Function
#End Region
#Region "Private"
    Private Shared Function Write(bm As Bitmap, message As String) As Bitmap
        Dim length As Integer = 0, offset As Integer = 0, dst As BitStream = Nothing, src As BitStream = Generator.ToBits(message)
        If (Generator.Length(bm) >= src.Count) Then
            For Each p As BitPosition In Generator.Positions
                For Each c As Channel In Generator.Channels
                    For y As Integer = 0 To bm.Height - 1
                        length = Generator.Read(src, dst, offset, bm.Width)
                        If (length > 0) Then
                            For x As Integer = 0 To dst.Count - 1
                                Generator.Write(bm, x, y, c, p, dst.ElementAt(x).Value)
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
    Private Shared Sub Write(bm As Bitmap, x As Integer, y As Integer, c As Channel, p As BitPosition, bit As BitValue)
        Dim result As BitStream = Nothing, pixel As Color = bm.GetPixel(x, y)
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
    Private Shared Function Read(src As BitStream, ByRef dst As BitStream, offset As Integer, len As Integer) As Integer
        Dim result As New BitStream
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
    Private Shared Function Read(bm As Bitmap) As IEnumerable(Of Byte)
        Dim collection As New List(Of Byte)
        For Each p As BitPosition In Generator.Positions
            For Each c As Channel In Generator.Channels
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
    Private Shared Function Read(bm As Bitmap, p As BitPosition, c As Channel, x As Integer, y As Integer) As Bit
        Dim current As Color = bm.GetPixel(x, y), value As Bit = Nothing
        Select Case c
            Case Channel.R : value = Generator.ToBits(current.R).ElementAt(p)
            Case Channel.G : value = Generator.ToBits(current.G).ElementAt(p)
            Case Channel.B : value = Generator.ToBits(current.B).ElementAt(p)
        End Select
        Return value
    End Function
    Private Shared Function Consume(src As List(Of Byte), length As Integer) As String
        Dim value As String = String.Empty
        If (length < src.Count - length) Then
            value = Strings.ChrW(Convert.ToByte(String.Concat(src.Take(length)), 2))
            src.RemoveRange(0, length)
        End If
        Return value
    End Function
    Private Shared Function ToBits(Value As Byte) As BitStream
        Dim buffer As New BitStream
        For Each x As Char In Convert.ToString(Value, 2).PadLeft(Bit.Length, "0"c).ToCharArray
            If (x = "0"c) Then
                buffer.Add(New Bit(BitValue.Zero))
            Else
                buffer.Add(New Bit(BitValue.One))
            End If
        Next
        Return buffer
    End Function
    Private Shared Function ToBits(Value As String) As BitStream
        Dim buffer As New BitStream
        For Each x As Char In Value.ToCharArray
            For Each y As Char In Convert.ToString(Strings.AscW(x), 2).PadLeft(Bit.Length, "0"c).ToCharArray
                If (y = "0"c) Then
                    buffer.Add(New Bit(BitValue.Zero))
                Else
                    buffer.Add(New Bit(BitValue.One))
                End If
            Next
        Next
        Return buffer
    End Function
    Private Shared Function ToByte(Stream As BitStream) As Byte
        Dim value As String = String.Empty
        For Each Bit As Bit In Stream
            If (Bit.Value = BitValue.Zero) Then
                value &= 0
            Else
                value &= 1
            End If
        Next
        If (Not value.Length = Bit.Length) Then Return 0
        Return Convert.ToByte(value, 2)
    End Function
    Private Shared Function Length(bm As Bitmap) As Integer
        Return bm.Width * bm.Height * [Enum].GetValues(GetType(BitPosition)).Length * [Enum].GetValues(GetType(Channel)).Length
    End Function
    Private Shared Function IsReadable(value As Char) As Boolean
        Return Strings.AscW(value) >= &H20 AndAlso Strings.AscW(value) <= &H7E Or value = Chr(&HA) Or value = Chr(&HD)
    End Function
#End Region
#Region "Properties"
    Public Shared ReadOnly Property Positions As Array
        Get
            Return [Enum].GetValues(GetType(BitPosition))
        End Get
    End Property
    Public Shared ReadOnly Property Channels As Array
        Get
            Return [Enum].GetValues(GetType(Channel))
        End Get
    End Property
#End Region
End Class

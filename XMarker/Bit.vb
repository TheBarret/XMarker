
Public Class Bit
    Public Property Value As BitValue
    Sub New(value As BitValue)
        Me.Value = value
    End Sub
    Public Sub Change(Value As BitValue)
        Me.Value = Value
    End Sub
End Class


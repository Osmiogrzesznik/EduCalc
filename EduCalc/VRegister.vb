Public Class VRegister
    Private Property Datainside As Double
    Public Property Name As String
    Private Property DisplayerHolder As Control
    Public Property isEmpty As Boolean = True


    Public Property Displayer As TextBox
        Set(ByVal displayerbox As TextBox)
            DisplayerHolder = displayerbox
            Name = displayerbox.Name
            displayerbox.Text = "bound to " & Me.Name
        End Set
        Get
            Return DisplayerHolder
        End Get

    End Property

    Public Sub Empty()
        isEmpty = True
        Datainside = 0
        Displayer.Text = "Empty"

    End Sub


    Public Property Data


        Set(ByVal value)

            isEmpty = False
            Datainside = value
            Displayer.Text = value

        End Set

        Get
            If isEmpty Then
                Return 0
            Else
                Return Datainside
            End If

        End Get

    End Property



End Class


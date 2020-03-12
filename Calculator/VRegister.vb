Friend Class VRegister
    Private datainside
    Public displayer

    Public Sub New(initvalue, displayerbox)
        displayer = displayerbox
        datainside = initvalue
        displayer.text = initvalue
    End Sub

    Public Property Data

        Set(value)

            datainside = value
            displayer.text = value
        End Set

        Get
            Return datainside
        End Get

    End Property



End Class


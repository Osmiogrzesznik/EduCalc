Imports System.Text
Imports VRegister

Public Class Form1
    Dim mem2 As VRegister = New VRegister()
    Dim mem As VRegister = New VRegister()
    Dim operation As VRegister = New VRegister()
    Dim result As VRegister = New VRegister()
    Dim clearScreen As Boolean = False
    Private Property strToBtnDict As Dictionary(Of String, Button) = New Dictionary(Of String, Button)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        mem2.Displayer = mem2_txtbx
        mem.Displayer = mem_txtbx
        operation.Displayer = operation_txtbx
        result.Displayer = result_txtbx

        For Each el In Controls
            If TypeOf el Is Button Then
                strToBtnDict.Add(el.text, el)
            End If
        Next
        strToBtnDict.Add("*", Btn_Mul)
        strToBtnDict.Add("A", Btn_AC)
        strToBtnDict.Add("/", Btn_Div)



        Btn_Test.PerformClick()


        AllClear()
    End Sub



    Private Sub Print(digit As Object)
        ' This is purely for aesthetical reasons - imitation of old digital display calculators behaviour
        If Screen.Text = "0" Then
            Screen.Text = ""
        End If
        Screen.Text += digit
    End Sub

    Private Function Read()
        If Screen.Text = "Error" Then Return "Error"
        Dim a As Double = Convert.ToDouble(Screen.Text)
        Return a
    End Function

    Private Sub AllClear()
        mem.Empty()
        mem2.Empty()
        operation.Empty()
        result.Empty()
        clearScreen = False
        Screen.Text = "0"
    End Sub

    Private Sub Btn_Dot_Click(sender As Object, e As EventArgs) Handles Btn_Dot.Click
        If Not Screen.Text.Contains(".") Then
            Screen.Text += "."
        End If
    End Sub

    Private Sub DigitButton_Clicked(sender As Object, e As EventArgs) Handles Btn_9.Click, Btn_8.Click, Btn_7.Click, Btn_6.Click, Btn_5.Click, Btn_4.Click, Btn_3.Click, Btn_2.Click, Btn_1.Click, Btn_0.Click
        If Screen.Text = "Error" Then
            Return
        End If

        If clearScreen Then
            Screen.Text = "0"
            clearScreen = False
        End If
        Print(sender.text)
    End Sub



    Private Sub Btn_AC_Click(sender As Object, e As EventArgs) Handles Btn_AC.Click
        AllClear()
    End Sub

    Private Sub Btn_C_Click(sender As Object, e As EventArgs) Handles Btn_C.Click
        Screen.Text = 0
        mem2.Empty()

    End Sub


    Private Function PerformOperation()
        'MessageBox.Show("mem2:" & mem2 & " mem:" & mem & "op:" & operation)
        Select Case operation.Data
            Case 1
                Return mem.Data + mem2.Data
            Case 2
                Return mem.Data - mem2.Data
            Case 3
                Return mem.Data * mem2.Data
            Case 4
                If mem2.Data = 0 Then
                    Throw New Exception
                Else
                    Return mem.Data / mem2.Data
                End If


            Case 0
                Return mem2.Data 'mem.Data
            Case Else
                Print("this is not implemented")
                Return 0

        End Select


    End Function

    Private Sub OperationBtn_Clicked(sender As Object, e As EventArgs) Handles Btn_Subt.Click, Btn_Add.Click, Btn_Div.Click, Btn_Mul.Click, Btn_prcnt.Click
        'Each operational or functional button has different tag property assigned in designer
        'every time this handler is invoked it assigns the value of invoking button tag to operation variable (of Vregistry class/type)


        'dont perform any further operation if Error occured until pressing AC button , which resets the state 
        If Screen.Text = "Error" Then
            Return
        End If


        If clearScreen And result.isEmpty Then
            mem2.Empty()
            result.Empty()
            ' this is true whe user pressed functional key immediaately following other functional key
            'in other words - if user did not put any value after pressing functional button
            ' for exaample sequence 3 + - + - 
            ' this way we prevent executing performoperation() and allow to change mind about the operation type in case of wrongly pressed button
            ' we allow to change the first operand as well ???
            ' mem is not empty !!!
            operation.Data = sender.Tag
            Return

        ElseIf mem.isEmpty Then
            'very beginning nothing else was pressed just number followed by sign
            mem.Data = Read()
            operation.Data = sender.Tag
            clearScreen = True
            Return 'is it really needed?
        ElseIf result.isEmpty Then
            'user entered second number and pressed func btn again - we need to perform calculation
            mem2.Data = Read()
            Try
                result.Data = PerformOperation()
            Catch ex As Exception
                AllClear()
                Print("Error")
                Return
            End Try
            mem.Data = result.Data
            mem2.Empty() 'we are clearing the mem2 for next number to come - we dont need to remember mem2 like in cas 2+2=====(2,4,6,8,10) because functional btns do not keep performing calc when repetaed press (2+2++++
            ' it would damage the ability to change the sign 3+-2=(1)
            Screen.Text = result.Data
            result.Empty()
        Else ' so mem is  not empty and result is not empty (this assumes that the mem2 is not empty as eq_btn do not erase it)
            operation.Data = sender.Tag
            ' mem.Data = result.Data ' redundant here or in eq_btn !!
            mem2.Empty()
            result.Empty()


        End If




        clearScreen = True
        operation.Data = sender.Tag


    End Sub

    Private Sub Btn_Eq_Click(sender As Object, e As EventArgs) Handles Btn_Eq.Click
        If Screen.Text = "Error" Then
            Return
        End If

        ''this means that somebody pressed = straight away or just after entering first number (which isnt in the memory yet
        If mem.isEmpty Then
            ' we simulate first complete calculation result of wich is  the number given so 7=(7)
            mem.Data = Read()
            result.Data = Read()
            Screen.Text = mem.Data() 'Redundant but completes the logical cycle
            clearScreen = True
            Return
        ElseIf mem2.isEmpty Then
            'if second number not entered assume like all calculators that it means that second operand is identical
            mem2.Data = Read()
        End If

        Try
            result.Data = PerformOperation()
        Catch ex As Exception
            AllClear()
            Print("Error")
            Return
        End Try

        mem.Data = result.Data
        'mem2.Empty() cannot clear mem2 because 2+==== fails giving 
        Screen.Text = result.Data
        'result.Empty()
        clearScreen = True
    End Sub




    Private Sub Btn_Test_Click(sender As Object, e As EventArgs) Handles Btn_Test.Click
        Dim summary As String
        'Dim testres(50)
        Dim testres As List(Of String) = New List(Of String)


        testres.Add(clickscenario("1+456(456) always clear screen for new input", seqFromString("1+456"), AddressOf Read, 456))
        testres.Add(clickscenario("1+1=(2)32 always clear screen for new input", seqFromString("1+1=32"), AddressOf Read, 32))
        testres.Add(clickscenario("1+2= standard", seqFromString("1+2="), AddressOf Read, 3))
        testres.Add(clickscenario("1+2+ func btn behave as eq_bt", seqFromString("1+2+"), AddressOf Read, 3))
        testres.Add(clickscenario("2*2+ func btn behave as eq_bt change op", seqFromString("2*2+"), AddressOf Read, 4))
        testres.Add(clickscenario("3+2+(5) func btn behave as eq_btn", seqFromString("3+2+"), AddressOf Read, 5))
        testres.Add(clickscenario("2+= assume that x+ means x+x", seqFromString("2+="), AddressOf Read, 4))
        testres.Add(clickscenario("2+=(4)=(6) assume every time = press", seqFromString("2+=="), AddressOf Read, 6))
        testres.Add(clickscenario("2*=(4)=(8) assume every time = press", seqFromString("2*=="), AddressOf Read, 8))
        testres.Add(clickscenario("7=(7)= equal should return given number", seqFromString("7=="), AddressOf Read, 7))
        testres.Add(clickscenario("1+2=(3)+4=(-1)", seqFromString("1+2=+4="), AddressOf Read, 7))
        testres.Add(clickscenario("4=(4)5=(5)+10+(15)", seqFromString("4=5=+10+"), AddressOf Read, 15))
        testres.Add(clickscenario("4=(4)5=(5)+10=(15)", seqFromString("4=5=+10="), AddressOf Read, 15))
        testres.Add(clickscenario("0=(0)=", seqFromString("0=="), AddressOf Read, 0))
        testres.Add(clickscenario("=(0)=(0)", seqFromString("=="), AddressOf Read, 0))
        testres.Add(clickscenario("0=(0)1=(1)", seqFromString("0=1="), AddressOf Read, 1))
        testres.Add(clickscenario("1=", seqFromString("1="), AddressOf Read, 1))
        testres.Add(clickscenario("3++(3)2=(5)", seqFromString("3++2="), AddressOf Read, 5))
        testres.Add(clickscenario("3+-(3)2=(5)", seqFromString("3+-2="), AddressOf Read, 1))
        testres.Add(clickscenario("3+-+-(3 all the time)2=(1)", seqFromString("3+-2="), AddressOf Read, 1))
        testres.Add(clickscenario("5+7+2=", seqFromString("5+7+2="), AddressOf Read, 14))
        testres.Add(clickscenario("5+7+", seqFromString("5+7+"), AddressOf Read, 12))
        testres.Add(clickscenario("5+7+2", seqFromString("5+7+2"), AddressOf Read, 2))
        testres.Add(clickscenario("5+7+2+", seqFromString("5+7+2+"), AddressOf Read, 14))
        testres.Add(clickscenario("5*0+2=", seqFromString("5*0+2="), AddressOf Read, 2))
        testres.Add(clickscenario("5*0=(0)+2=", seqFromString("5*0=+2="), AddressOf Read, 2))
        testres.Add(clickscenario("5*0=(0)2=", seqFromString("5*0+2="), AddressOf Read, 2))
        testres.Add(clickscenario("5/0=", seqFromString("5/0="), AddressOf ReadText, "Error"))
        testres.Add(clickscenario("(TO DO:math operation order)5+2*0-", seqFromString("5+2*0-"), AddressOf Read, 5))




        For Each tr In testres
            summary &= tr
        Next

        MessageBox.Show(summary)

    End Sub

    Private Function ReadText()
        Return Screen.Text
    End Function

    Private Function seqFromString(str As String) As Array
        Dim Out As ArrayList = New ArrayList

        For Each ch In str
            Out.Add(strToBtnDict(ch.ToString()))

        Next
        Return Out.ToArray()

    End Function



    Private Function clickscenario(Title As String, ByVal Arr As Array, ResultFunc As Func(Of Object), ExpectedResult As Object) As String
        AllClear()
        Dim report As String
        For Each btn In Arr
            btn.PerformClick()
        Next
        ' MessageBox.Show(ResultFunc().ToString)
        If ResultFunc() = ExpectedResult Then
            report = "ok:" & Title & Environment.NewLine
        Else
            report = "__FAILED : " & Title & "giving " & ResultFunc().ToString & " when expected " & ExpectedResult & Environment.NewLine
        End If

        Return report
    End Function

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles RegViewCheck.Click
        RegViewCheck.Checked = Not RegViewCheck.Checked

        If RegViewCheck.Checked Then
            mem2_txtbx.Show()
            mem_txtbx.Show()
            result_txtbx.Show()
            operation_txtbx.Show()
            Btn_Test.Show()
            Label1.Show()
            Label2.Show()
            Label3.Show()
            Label4.Show()
            ClientSize = New Size(433, 320)
        Else
            mem2_txtbx.Hide()
            mem_txtbx.Hide()
            result_txtbx.Hide()
            operation_txtbx.Hide()
            Btn_Test.Hide()
            Label1.Hide()
            Label2.Hide()
            Label3.Hide()
            Label4.Hide()
            ClientSize = New Size(240, 320)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btn_copyclip.Click
        Clipboard.SetText(Screen.Text)
    End Sub
End Class

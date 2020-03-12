Imports System.Text
Imports VRegister

Public Class Form1
    Dim mem2 As VRegister = New VRegister(0, mem2_txtbx)
    Dim mem As VRegister = New VRegister(0, mem_txtbx)
    Dim operation As VRegister = New VRegister(0, operation_txtbx)
    Dim result As VRegister = New VRegister(0, result_txtbx)

    Private Sub Print(digit As Object)
        ' This is purely for aesthetical reasons - imitation of old digital display calculators behaviour
        If Screen.Text = "0" Then
            Screen.Text = ""
        End If
        Screen.Text += digit
    End Sub

    Private Function Read()
        Dim a As Double = Convert.ToDouble(Screen.Text)
        ' MessageBox.Show(a)
        Return a
    End Function

    Private Sub AllClear()
        mem.Data = 0
        mem2.Data = 0
        operation.Data = 0
        Screen.Text = "0"
    End Sub

    Private Sub Btn_Dot_Click(sender As Object, e As EventArgs) Handles Btn_Dot.Click
        If Not Screen.Text.Contains(".") Then
            Screen.Text += "."
        End If
    End Sub

    Private Sub DigitButton_Clicked(sender As Object, e As EventArgs) Handles Btn_9.Click, Btn_8.Click, Btn_7.Click, Btn_6.Click, Btn_5.Click, Btn_4.Click, Btn_3.Click, Btn_2.Click, Btn_1.Click, Btn_0.Click
        'Screen.Text = mem.ToString()
        Print(sender.text)
        mem.Data = Read()
    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AllClear()

    End Sub

    Private Sub Btn_AC_Click(sender As Object, e As EventArgs) Handles Btn_AC.Click
        AllClear()
    End Sub

    Private Sub PerformOperation()
        'MessageBox.Show("mem2:" & mem2 & " mem:" & mem & "op:" & operation)
        Select Case operation.Data
            Case 1
                mem2.Data += mem.Data
            Case 2
                mem2.Data -= mem.Data
            Case 3
                mem2.Data *= mem.Data
            Case 4
                Try
                    mem2.Data = mem2.Data / mem.Data
                Catch
                    Print("Error")
                End Try
            Case 5
                Print("% not implemented")
            Case 0
                mem2.Data = mem.Data
            Case Else
                Return

        End Select


    End Sub

    Private Sub OperationBtn_Clicked(sender As Object, e As EventArgs) Handles Btn_Subt.Click, Btn_Add.Click, Btn_Div.Click, Btn_Mul.Click, Btn_prcnt.Click
        'perform operation existing in memory first whther it is a just assignment or any else
        ' this will make it behave as real calc when you keep pressing = at the start nothing happens
        If mem2.Data <> 0 Then
            PerformOperation()
        End If
        mem2.Data = Read()

        operation = sender.Tag
        Screen.Text = mem2.ToString()
        mem.Data = 0

    End Sub

    Private Sub Btn_Eq_Click(sender As Object, e As EventArgs) Handles Btn_Eq.Click
        PerformOperation()
        Screen.Text = mem2.Data.ToString()
        'mem2 = 0
    End Sub
End Class

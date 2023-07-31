Imports System.IO
Imports System.Runtime.ConstrainedExecution
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.TaskBand

Public Class Form1

    Dim TypeCount(5) As Integer
    Private Generator As System.Random = New System.Random()
    Dim curStroke(1) As Integer
    Dim curStrokeSpeed As Integer
    Dim scriptTime As Integer = 0
    Dim CurPos As Integer = 0
    Dim Style As Integer
    Dim TravelPoint As Integer
    Dim TotalActions As Integer

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If lblDemo.Visible = False Then
            scriptTime = 0
            curStroke(0) = 0
            curStroke(1) = 0
            curStrokeSpeed = 0
            Style = 0
            tbOutput.Text = "{""actions"":[" + vbCrLf
            TypeCount(0) = 0 ' Normal
            TypeCount(1) = 0 ' Group Of X
            TypeCount(2) = 0 ' Stroke Pause
            TypeCount(3) = 0 ' Pause
            TypeCount(4) = 0 ' Randome
            TypeCount(5) = 0 ' Vibrate

            Do While scriptTime < CInt(tbLength.Text) * 60 * 1000
                Dim curStyle As Integer = SelectGroupStyle()

                Select Case curStyle
                    Case 1
                        If cbNormal.Checked Then
                            NormalStroke()
                        End If

                    'Case 2
                    '    If cbGroupOfThree.Checked Then
                    '        TypeCount(1) += 1
                    '        Dim GroupTotal As Integer = GroupRepeat() + GroupVar(CInt(tb3GroupPerGroup.Text))
                    '        Dim groupCount As Integer = 0
                    '        Dim curStroke() As Integer = GroupstrokeLengthBase()
                    '        Dim StartAtBottom As Boolean = True
                    '        Do While groupCount < GroupTotal
                    '            If CInt(tbRandStroke.Text) >= Generator.Next(1, 100) Then
                    '                RandomStroke()
                    '            End If
                    '            curStrokeSpeed = StrokeTimeVar(StrokeTime())

                    '            Dim StepTime1 As Integer = scriptTime + curStrokeSpeed
                    '            Dim StepTime2 As Integer = StepTime1 + curStrokeSpeed
                    '            Dim StepTime3 As Integer = StepTime2 + curStrokeSpeed
                    '            Dim StepTime4 As Integer = StepTime3 + ((StepTime3 - scriptTime))

                    '            If StartAtBottom Then
                    '                writeStroke(StepTime1, curStroke(0))
                    '                writeStroke(StepTime2, curStroke(1))
                    '                writeStroke(StepTime3, curStroke(0))
                    '                writeStroke(StepTime4, curStroke(0))
                    '                CurPos = curStroke(1)
                    '                StartAtBottom = False
                    '            Else
                    '                writeStroke(StepTime1, curStroke(1))
                    '                writeStroke(StepTime2, curStroke(0))
                    '                writeStroke(StepTime3, curStroke(1))
                    '                writeStroke(StepTime4, curStroke(1))
                    '                CurPos = curStroke(1)
                    '                StartAtBottom = True
                    '            End If
                    '            scriptTime = StepTime4
                    '            groupCount += 1
                    '        Loop
                    '    End If

                    Case 3
                        If cbPauseStroke.Checked Then
                            StrokePause()
                        End If
                    Case 4
                        If cbVibrate.Checked Then
                            Vibrate()
                        End If
                    Case 5
                        If cbPause.Checked Then
                            Pause()
                        End If
                End Select
                Style = curStyle
            Loop
            scriptTime += 1000
            tbOutput.Text += "{""at"":" + scriptTime.ToString + ",""pos"":50}" + vbCrLf
            tbOutput.Text += "]" + vbCrLf + "}"
            lblNorm.Text = "Normal Stroke: " + TypeCount(0).ToString
            lblGroup.Text = "Group Of X: " + TypeCount(1).ToString
            lblFast.Text = "Stroke Pause: " + TypeCount(2).ToString
            lblPause.Text = "Pause: " + TypeCount(3).ToString
            lblRandome.Text = "Random Strokes: " + TypeCount(4).ToString
            lblVibrate.Text = "Vibration: " + TypeCount(5).ToString
            lblTotalActions.Text = "Total Acrions: " + TotalActions.ToString
            gbBreakDown.Visible = True
        End If
    End Sub

    Public Sub Transition(NewStroke() As Integer, NewSpeed As Integer)
        Dim TransStrokeCount As Integer = Generator.Next(2, 6)
        Dim StrokeStep(1) As Integer
        Dim SpeedStep As Integer = 0
        Dim StrokeCount As Integer = 1
        StrokeStep(0) = (NewStroke(0) - curStroke(0)) / TransStrokeCount
        StrokeStep(1) = (NewStroke(1) - curStroke(1)) / TransStrokeCount
        SpeedStep = (NewSpeed - curStrokeSpeed) / TransStrokeCount

        Do While StrokeCount <= TransStrokeCount
            Dim tempStroke(1) As Integer
            tempStroke(0) = curStroke(0) + (StrokeStep(0) * StrokeCount)
            tempStroke(1) = curStroke(1) + (StrokeStep(1) * StrokeCount)
            Dim tempSpeed As Integer = curStrokeSpeed + (SpeedStep * StrokeCount)
            Dim Time1 As Integer = scriptTime + tempSpeed
            Dim Time2 As Integer = Time1 + tempSpeed
            writeStroke(Time1, tempStroke(0))
            writeStroke(Time2, tempStroke(1))
            scriptTime = Time2
            StrokeCount += 1
        Loop
    End Sub

    Public Sub RandomStroke()
        If cbRandomStroke.Checked Then
            TypeCount(4) += 1
            Dim StrokeStyle As Integer = Generator.Next(1, 2)
            Dim StrokeCount As Integer = Generator.Next(5, 15)
            Select Case StrokeStyle
                Case 1 'Full Stroke


                    Dim randStroke(1) As Integer
                    randStroke(0) = CInt(tbMaxDepth.Text)
                    randStroke(1) = CInt(tbMinDepth.Text)
                    Dim randSpeed As Integer = StrokeTime(randStroke)

                    Dim curStrokecount As Integer = 0
                    Do While curStrokecount < StrokeCount
                        scriptTime += StrokeTimeVar(randSpeed)
                        writeStroke(scriptTime, randStroke(0))
                        scriptTime += StrokeTimeVar(randSpeed)
                        writeStroke(scriptTime, randStroke(1))
                        curStrokecount += 1
                    Loop

                    'Case 2 'Top 1/2


                    '    Dim randStroke(1) As Integer
                    '    randStroke(0) = CInt(tbMaxDepth.Text)
                    '    randStroke(1) = (CInt(tbMaxDepth.Text) - CInt(tbMinDepth.Text)) / 2 + CInt(tbMinDepth.Text)
                    '    Dim randSpeed As Integer = StrokeTime(randStroke)
                    '    Dim curStrokecount As Integer = 0
                    '    Do While curStrokecount < StrokeCount
                    '        scriptTime += StrokeTimeVar(randSpeed)
                    '        writeStroke(scriptTime, randStroke(0))
                    '        scriptTime += StrokeTimeVar(randSpeed)
                    '        writeStroke(scriptTime, randStroke(1))
                    '        curStrokecount += 1
                    '    Loop

                    'Case 3 'Bottom 1/2


                    '    Dim randStroke(1) As Integer
                    '    randStroke(0) = (CInt(tbMaxDepth.Text) - CInt(tbMinDepth.Text)) / 2 + CInt(tbMinDepth.Text)
                    '    randStroke(1) = CInt(tbMinDepth.Text)
                    '    Dim randSpeed As Integer = StrokeTime(randStroke)
                    '    Dim curStrokecount As Integer = 0
                    '    Do While curStrokecount < StrokeCount
                    '        scriptTime += StrokeTimeVar(randSpeed)
                    '        writeStroke(scriptTime, randStroke(0))
                    '        scriptTime += StrokeTimeVar(randSpeed)
                    '        writeStroke(scriptTime, randStroke(1))
                    '        curStrokecount += 1
                    '    Loop
                    'Case 4 'Full Stroke Fastest
                    '    Dim randSpeed As Integer = 1000 \ CInt(tbStrokeMax.Text)

                    '    Dim randStroke(1) As Integer
                    '    randStroke(0) = CInt(tbMinDepth.Text)
                    '    randStroke(1) = CInt(tbMaxDepth.Text)

                    '    Dim curStrokecount As Integer = 0
                    '    Do While curStrokecount < StrokeCount
                    '        scriptTime += StrokeTimeVar(randSpeed)
                    '        writeStroke(scriptTime, randStroke(0))
                    '        scriptTime += StrokeTimeVar(randSpeed)
                    '        writeStroke(scriptTime, randStroke(1))
                    '        curStrokecount += 1
                    '    Loop
                    'Case 5 'Fast Then Off
                    '    Dim GroupTotal As Integer = Generator.Next(1, 6)
                    '    Dim GroupCount As Integer = 0
                    '    Dim RandStroke() As Integer = GroupstrokeLengthBase()
                    '    Dim randSpeed As Integer = 1000 \ CInt(tbStrokeMax.Text)
                    '    Dim StartAtBottom As Boolean = True
                    '    Do While GroupCount < GroupTotal
                    '        Dim Bounce As Integer = 0
                    '        Dim MaxBounce As Integer = GroupVar(15)
                    '        If CInt(tbRandStroke.Text) >= Generator.Next(1, 100) Then
                    '            RandomStroke()
                    '        End If
                    '        Do While Bounce <= MaxBounce
                    '            scriptTime += randSpeed
                    '            Dim tStep As Integer = scriptTime + randSpeed
                    '            writeStroke(scriptTime, RandStroke(0))
                    '            writeStroke(tStep, RandStroke(1))
                    '            CurPos = RandStroke(1)
                    '            scriptTime = tStep
                    '            Bounce += 1
                    '        Loop
                    '        scriptTime += Generator.Next(500, 2000)
                    '        writeStroke(scriptTime, CurPos)
                    '        GroupCount += 1
                    '    Loop
            End Select
        End If

    End Sub

    Private Sub writeStroke(EndTime As Integer, EndPos As Integer) 'writes a 1/2 stroke or a pause

        If EndPos < 0 Then EndPos = 0
        If EndPos > 115 Then EndPos = 115

        tbOutput.Text += "{""at"":" + EndTime.ToString + ",""pos"":" + EndPos.ToString + "}," + vbCrLf
        tbOutput.Select(tbOutput.Text.Length + 1, 1)
        tbOutput.Refresh()
        Dim TotalTime As Integer = CInt(tbLength.Text) * 60 * 1000
        Dim pf As Integer = (scriptTime / TotalTime) * 100
        TotalActions += 1
        Try
            ProgressBar1.Value = pf
        Catch ex As Exception
            ProgressBar1.Value = 100
        End Try
        ProgressBar1.PerformStep()
        ProgressBar1.Update()
    End Sub

    Private Function GroupRepeat() As Integer

        Return Generator.Next(1, CInt(tbGroupRepeat.Text) + 1)
    End Function

    Private Function StrokeTime(curStroke() As Integer) As Integer 'Gets 1/2 full stroke time in ms

        Dim mmPS As Integer = Generator.Next(CInt(tbStrokeMin.Text), CInt(tbStrokeMax.Text))
        Dim Dist As Integer = curStroke(0) - curStroke(1)
        Dim curStrokeTime As Integer = (Dist / mmPS) * 1000

        Return curStrokeTime
    End Function

    Private Function StrokeTimeVar(Basetime As Integer) As Integer 'Adds up to a .1 sec varibility in the stroke
        Dim ChangeTime As Integer = Basetime * (CInt(tbVar.Text) / 100)
        Dim Time As Integer = Generator.Next(Basetime - ChangeTime, Basetime + ChangeTime)
        Return Time
    End Function

    Private Function SelectGroupStyle() As Integer 'Selects the group style

        Dim CurCount As Integer = 1
        Dim RandChoice As Integer = Generator.Next(1, 101)
        Dim Choice As Integer = 0

        If cbNormal.Checked Then
            If RandChoice > CurCount And RandChoice < CurCount + CInt(tbNormalPercent.Text) Then
                Choice = 1
            End If
            CurCount += CInt(tbNormalPercent.Text)
        End If
        If cbGroupOfThree.Checked Then
            If RandChoice > CurCount And RandChoice < CurCount + CInt(tbGOXPercent.Text) Then
                Choice = 2
            End If
            CurCount += CInt(tbGOXPercent.Text)
        End If
        If cbPauseStroke.Checked Then
            If RandChoice > CurCount And RandChoice < CurCount + CInt(tbStrokePausePercent.Text) Then
                Choice = 3
            End If
            CurCount += CInt(tbStrokePausePercent.Text)
        End If
        If cbVibrate.Checked Then
            If RandChoice > CurCount And RandChoice < CurCount + CInt(tbVibratePercent.Text) Then
                Choice = 4
            End If
            CurCount += CInt(tbVibratePercent.Text)
        End If
        If cbPause.Checked Then
            If RandChoice > CurCount And RandChoice < CurCount + CInt(tbPausePercent.Text) Then
                Choice = 5
            End If
            CurCount += CInt(tbPausePercent.Text)
        End If
        Return Choice
    End Function

    Private Function GroupVar(Total As Integer) As Integer ' adds up to 10% more strokes for each group

        Dim Ver As Integer = Total * 0.1
        Return Total + Generator.Next(0, Ver + 1)
    End Function

    Private Function GroupstrokeLengthBase() As Integer() ' Gets the Base Group Stroke Length

        Dim Good As Boolean = False
        Dim Stroke(1) As Integer
        Dim MinLength As Integer = (CInt(tbMaxDepth.Text) - CInt(tbMinDepth.Text)) * (CInt(tbMinStrokeLength.Text) / 100)
        Do While Good = False
            Dim Max As Integer = Generator.Next(CInt(tbMinDepth.Text), CInt(tbMaxDepth.Text) + 1)
            Dim Min = Generator.Next(CInt(tbMinDepth.Text), CInt(tbMaxDepth.Text) + 1)
            If Max - Min >= MinLength Then
                Stroke(0) = Max
                Stroke(1) = Min
                Good = True
            End If
        Loop
        Return Stroke
    End Function

    Private Function GroupstrokeLengthVar(Stroke() As Integer) As Integer() ' adds or subtracts stroke length varibility
        Dim Good As Boolean = False
        Dim nStroke(1) As Integer
        Dim LoopCount As Integer = 0
        Do While Good = False And LoopCount < 15
            Dim MaxVar As Integer = Stroke(0) + Generator.Next(Stroke(0) * ((CInt(tbVar.Text) / 100) * -1), Stroke(0) * (CInt(tbVar.Text) / 100))
            Dim MinVar As Integer = Stroke(1) + Generator.Next(Stroke(1) * ((CInt(tbVar.Text) / 100) * -1), Stroke(1) * (CInt(tbVar.Text) / 100))
            Dim MinDist As Integer = (CInt(tbMaxDepth.Text) - CInt(tbMinDepth.Text)) * (CInt(tbMinStrokeLength.Text) / 100)
            If MaxVar > MinVar Then
                If MaxVar > 100 Then
                    nStroke(0) = 100
                Else
                    nStroke(0) = MaxVar
                End If
                If MinVar < 0 Then
                    nStroke(1) = 0
                Else
                    nStroke(1) = MinVar
                End If
                Good = True

                If Good Then
                    If (MaxVar - MinVar) < MinDist Then
                        Good = False
                    End If
                End If
            End If
            LoopCount += 1
        Loop
        If LoopCount >= 15 Then
            Return Stroke
        Else
            Return nStroke
        End If

    End Function

    Public Sub BounceStroke(ThisStroke() As Integer, Bottom As Boolean)
        Dim bouncecount As Integer = Generator.Next(1, 3)
        Dim bounce As Integer = 0
        If Bottom Then
            Do While bounce < bouncecount
                scriptTime += 70
                writeStroke(scriptTime, ThisStroke(1) + (ThisStroke(0) * 0.2))
                scriptTime += 70
                writeStroke(scriptTime, ThisStroke(1))
                bounce += 1
            Loop
        Else
            Do While bounce < bouncecount
                scriptTime += 70
                writeStroke(scriptTime, ThisStroke(0) - (ThisStroke(0) * 0.2))
                scriptTime += 70
                writeStroke(scriptTime, ThisStroke(0))
                bounce += 1
            Loop
        End If

    End Sub


#Region "Stroke Builder"

    Public Sub NormalStroke()
        TypeCount(0) += 1
        Dim groupCount As Integer = 0
        Dim groupTotal As Integer = GroupRepeat()
        Do While groupCount <= groupTotal
            Dim StrokeCount As Integer = 0
            Dim StrokeMaxCount As Integer = GroupVar(CInt(tbNormalStrokePerGroup.Text))
            Dim NewStroke() As Integer = GroupstrokeLengthBase()
            Dim NewStrokeSpeed As Integer = StrokeTime(NewStroke)

            Transition(NewStroke, NewStrokeSpeed)

            curStroke = NewStroke
            curStrokeSpeed = NewStrokeSpeed


            Do While StrokeCount < StrokeMaxCount

                If CInt(tbRandStroke.Text) >= Generator.Next(1, 101) Then
                    RandomStroke()
                End If

                Dim ThisStroke(1) As Integer
                If cbNormalDistVar.Checked Then
                    ThisStroke = GroupstrokeLengthVar(curStroke)
                Else
                    ThisStroke = curStroke
                End If
                If cbNormalSpeedVar.Checked Then
                    scriptTime += StrokeTimeVar(curStrokeSpeed)
                Else
                    scriptTime += curStrokeSpeed
                End If


                writeStroke(scriptTime, ThisStroke(0))

                If CInt(tbNormalBounceLikelyhood.Text) > Generator.Next(1, 101) And cbNormalBounceTop.Checked Then
                    BounceStroke(ThisStroke, False)
                End If
                If cbNormalSpeedVar.Checked Then
                    scriptTime += StrokeTimeVar(curStrokeSpeed)
                Else
                    scriptTime += curStrokeSpeed
                End If

                writeStroke(scriptTime, ThisStroke(1))

                If CInt(tbNormalBounceLikelyhood.Text) > Generator.Next(1, 101) And cbNormalBounceBottom.Checked Then
                    BounceStroke(ThisStroke, True)
                End If

                StrokeCount += 1
                CurPos = curStroke(0)

                If cbNormalTravel.Checked Then
                    curStroke = ThisStroke
                End If

            Loop
            groupCount += 1
        Loop
    End Sub

    Public Sub StrokePause()
        TypeCount(2) += 1
        Dim groupCount As Integer = 0
        Dim groupTotal As Integer = GroupRepeat()
        Do While groupCount <= groupTotal
            Dim StrokeCount As Integer = 0
            Dim StrokeMaxCount As Integer = GroupVar(CInt(tbPauseStrokeStrokePerGroup.Text))
            Dim NewStroke() As Integer = GroupstrokeLengthBase()
            Dim NewStrokeSpeed As Integer = StrokeTime(NewStroke)

            Transition(NewStroke, NewStrokeSpeed)

            curStroke = NewStroke
            curStrokeSpeed = NewStrokeSpeed

            Dim BottomPause As Boolean = True
            If cbPauseStrokePauseBottom.Checked And Not cbPauseStrokePauseTop.Checked Then
                BottomPause = True
            ElseIf Not cbPauseStrokePauseBottom.Checked And cbPauseStrokePauseTop.Checked Then
                BottomPause = False
            ElseIf cbPauseStrokePauseBottom.Checked And cbPauseStrokePauseTop.Checked Then
                BottomPause = CBool(Generator.Next(0, 2))
            ElseIf Not cbPauseStrokePauseBottom.Checked And Not cbPauseStrokePauseTop.Checked Then
                BottomPause = True
            End If

            Do While StrokeCount < StrokeMaxCount + 1

                If CInt(tbRandStroke.Text) >= Generator.Next(1, 101) Then
                    RandomStroke()
                End If

                Dim ThisStroke(1) As Integer
                If cbPauseStrokeDistVar.Checked Then
                    ThisStroke = GroupstrokeLengthVar(curStroke)
                Else
                    ThisStroke = curStroke
                End If
                If cbPauseStrokeSpeedVar.Checked Then
                    scriptTime += StrokeTimeVar(curStrokeSpeed)
                Else
                    scriptTime += curStrokeSpeed
                End If

                If BottomPause Then
                    writeStroke(scriptTime, ThisStroke(0))

                    If CInt(tbPauseStrokeBounceLikelyhood.Text) > Generator.Next(1, 101) And cbPauseStrokeAllowBounce.Checked And cbPauseStrokeBounceTop.Checked Then
                        BounceStroke(ThisStroke, False)
                    End If

                    If cbNormalSpeedVar.Checked Then
                        scriptTime += StrokeTimeVar(curStrokeSpeed)
                    Else
                        scriptTime += curStrokeSpeed
                    End If

                    writeStroke(scriptTime, ThisStroke(1))

                    If CInt(tbPauseStrokeBounceLikelyhood.Text) > Generator.Next(1, 101) And cbPauseStrokeAllowBounce.Checked And cbPauseStrokeBounceBottom.Checked Then
                        BounceStroke(ThisStroke, True)
                    End If

                    scriptTime += curStrokeSpeed
                    writeStroke(scriptTime, ThisStroke(1))
                Else
                    writeStroke(scriptTime, ThisStroke(1))

                    If CInt(tbPauseStrokeBounceLikelyhood.Text) > Generator.Next(1, 101) And cbPauseStrokeAllowBounce.Checked And cbPauseStrokeBounceBottom.Checked Then
                        BounceStroke(ThisStroke, True)
                    End If

                    If cbNormalSpeedVar.Checked Then
                        scriptTime += StrokeTimeVar(curStrokeSpeed)
                    Else
                        scriptTime += curStrokeSpeed
                    End If

                    writeStroke(scriptTime, ThisStroke(0))

                    If CInt(tbPauseStrokeBounceLikelyhood.Text) > Generator.Next(1, 101) And cbPauseStrokeAllowBounce.Checked And cbPauseStrokeBounceTop.Checked Then
                        BounceStroke(ThisStroke, False)
                    End If

                    scriptTime += curStrokeSpeed
                    writeStroke(scriptTime, ThisStroke(0))
                End If

                StrokeCount += 1
                CurPos = curStroke(0)

                If cbPauseStrokeTravel.Checked Then
                    curStroke = ThisStroke
                End If

            Loop
            groupCount += 1
        Loop
    End Sub

    Public Sub Vibrate()
        If scriptTime > 150000 Or CInt(tbVibratePercent.Text) > 80 Then
            TypeCount(5) += 1
            TravelPoint = Generator.Next(CInt(tbMinDepth.Text) + 10, CInt(tbMaxDepth.Text - 10))
            Dim CurPoint As Integer = 0
            Dim TravelD As Integer = 0

            Dim stepDist As Integer = 0
            Dim stepCountTotal As Integer = 0

            Dim NewStroke(1) As Integer
            Dim NewStrokeSpeed As Integer = 50

            NewStroke(0) = TravelPoint - 5
            NewStroke(1) = TravelPoint + 5

            Transition(NewStroke, NewStrokeSpeed)
            curStroke = NewStroke
            curStrokeSpeed = NewStrokeSpeed
            CurPoint = TravelPoint

            Dim stopTime As Integer = scriptTime + Generator.Next((CInt(tbVibrationTime.Text) * 1000) / 2, (CInt(tbVibrationTime.Text) * 1000))

            If cbVibrationAllowTravel.Checked Then
                Do While scriptTime < stopTime
                    stepCountTotal = Generator.Next(4000, 6000) \ 100

                    Do While TravelPoint > CurPoint - 30 And TravelPoint < CurPoint + 30
                        TravelPoint = Generator.Next(CInt(tbMinDepth.Text) + 10, CInt(tbMaxDepth.Text - 10))
                    Loop

                    If TravelPoint > CurPoint Then
                        TravelD = 0
                    Else
                        TravelD = 1
                    End If

                    If TravelD = 0 Then
                        stepDist = (TravelPoint - CurPoint) / stepCountTotal
                    Else
                        stepDist = (TravelPoint + CurPoint) / stepCountTotal
                    End If
                    Do While CurPoint <> TravelPoint
                        NewStroke(0) = CurPoint - 5
                        NewStroke(1) = CurPoint + 5
                        scriptTime += curStrokeSpeed
                        writeStroke(scriptTime, curStroke(0))
                        scriptTime += curStrokeSpeed
                        writeStroke(scriptTime, curStroke(1))

                        If TravelD = 0 Then
                            CurPoint += stepDist
                            If CurPoint > TravelPoint Then CurPoint = TravelPoint
                        Else
                            CurPoint -= stepDist
                            If CurPoint < TravelPoint Then CurPoint = TravelPoint
                        End If
                    Loop
                Loop
            Else
                Do While scriptTime < stopTime
                    scriptTime += curStrokeSpeed
                    writeStroke(scriptTime, curStroke(0))
                    scriptTime += curStrokeSpeed
                    writeStroke(scriptTime, curStroke(1))
                Loop
            End If

            CurPos = curStroke(0)
        End If
    End Sub

    Public Sub Pause()
        TypeCount(3) += 1
        Dim NewStroke(1) As Integer
        NewStroke(0) = 0
        NewStroke(1) = 0
        Dim NewStrokeSpeed As Integer = 0
        Transition(NewStroke, NewStrokeSpeed)
        curStroke = NewStroke
        curStrokeSpeed = NewStrokeSpeed
        scriptTime += Generator.Next(1, CInt(tbPauseLength.Text)) * 1000
        writeStroke(scriptTime, NewStroke(1))
    End Sub
#End Region
#Region "Form Controls"


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CalcTotalPercent()
    End Sub

    Private Sub cbNormal_CheckedChanged(sender As Object, e As EventArgs) Handles cbNormal.CheckedChanged
        CalcTotalPercent()
    End Sub

    Private Sub cbGroupOfThree_CheckedChanged(sender As Object, e As EventArgs) Handles cbGroupOfThree.CheckedChanged
        CalcTotalPercent()
    End Sub

    Private Sub cbPauseStroke_CheckedChanged(sender As Object, e As EventArgs) Handles cbPauseStroke.CheckedChanged
        CalcTotalPercent()
    End Sub

    Private Sub cbVibrate_CheckedChanged(sender As Object, e As EventArgs) Handles cbVibrate.CheckedChanged
        CalcTotalPercent()
    End Sub

    Private Sub cbPause_CheckedChanged(sender As Object, e As EventArgs) Handles cbPause.CheckedChanged
        CalcTotalPercent()
    End Sub

    Private Sub tbNormalPercent_TextChanged(sender As Object, e As EventArgs) Handles tbNormalPercent.TextChanged
        CalcTotalPercent()
    End Sub

    Private Sub tbGOXPercent_TextChanged(sender As Object, e As EventArgs) Handles tbGOXPercent.TextChanged
        CalcTotalPercent()
    End Sub

    Private Sub tbStrokePausePercent_TextChanged(sender As Object, e As EventArgs) Handles tbStrokePausePercent.TextChanged
        CalcTotalPercent()
    End Sub

    Private Sub tbVibragePercent_TextChanged(sender As Object, e As EventArgs) Handles tbVibratePercent.TextChanged
        CalcTotalPercent()
    End Sub

    Private Sub tbPauseGroup_TextChanged(sender As Object, e As EventArgs) Handles tbPausePercent.TextChanged
        CalcTotalPercent()
    End Sub

    Private Sub CalcTotalPercent()
        Try
            Dim Total As Integer = 0
            If cbNormal.Checked Then
                Total += CInt(tbNormalPercent.Text)
            End If
            If cbGroupOfThree.Checked Then
                Total += CInt(tbGOXPercent.Text)
            End If
            If cbPauseStroke.Checked Then
                Total += CInt(tbStrokePausePercent.Text)
            End If
            If cbVibrate.Checked Then
                Total += CInt(tbVibratePercent.Text)
            End If
            If cbPause.Checked Then
                Total += CInt(tbPausePercent.Text)
            End If
            lblTotalPercent.Text = Total.ToString
            If Not Total = 100 Then
                lblTotalPercent.ForeColor = Color.Red
            Else
                lblTotalPercent.ForeColor = Color.Black
            End If
        Catch ex As Exception
            lblTotalPercent.Text = "Error"
            lblTotalPercent.ForeColor = Color.Red
        End Try
    End Sub

    Private Sub cbPauseStrokePauseTop_CheckedChanged(sender As Object, e As EventArgs) Handles cbPauseStrokePauseTop.CheckedChanged
        If cbPauseStrokePauseTop.Checked Then

        Else
            cbPauseStrokePauseBottom.Checked = True
        End If
    End Sub

    Private Sub cbPauseStrokePauseBottom_CheckedChanged(sender As Object, e As EventArgs) Handles cbPauseStrokePauseBottom.CheckedChanged
        If cbPauseStrokePauseBottom.Checked Then

        Else
            cbPauseStrokePauseTop.Checked = True
        End If
    End Sub

    Private Sub cbPauseStrokeBounceTop_CheckedChanged(sender As Object, e As EventArgs) Handles cbPauseStrokeBounceTop.CheckedChanged
        If cbPauseStrokeBounceTop.Checked Then

        Else
            cbPauseStrokeBounceBottom.Checked = True
        End If
    End Sub

    Private Sub cbPauseStrokeBounceBottom_CheckedChanged(sender As Object, e As EventArgs) Handles cbPauseStrokeBounceBottom.CheckedChanged
        If cbPauseStrokeBounceBottom.Checked Then

        Else
            cbPauseStrokeBounceTop.Checked = True
        End If
    End Sub

    Private Sub cbPauseStrokeAllowBounce_CheckedChanged(sender As Object, e As EventArgs) Handles cbPauseStrokeAllowBounce.CheckedChanged
        If cbPauseStrokeAllowBounce.Checked Then
            cbPauseStrokeBounceBottom.Visible = True
            cbPauseStrokeBounceTop.Visible = True
            lblPauseStrokeBounceLikelyhood.Visible = True
            tbPauseStrokeBounceLikelyhood.Visible = True
        Else
            cbPauseStrokeBounceBottom.Visible = False
            cbPauseStrokeBounceTop.Visible = False
            lblPauseStrokeBounceLikelyhood.Visible = False
            tbPauseStrokeBounceLikelyhood.Visible = False
        End If
    End Sub

    Private Sub cbNormalBounce_CheckedChanged(sender As Object, e As EventArgs) Handles cbNormalBounce.CheckedChanged
        If cbNormalBounce.Checked Then
            cbNormalBounceBottom.Visible = True
            cbNormalBounceTop.Visible = True
            lblNormalBounceLikelyhood.Visible = True
            tbNormalBounceLikelyhood.Visible = True
        Else
            cbNormalBounceBottom.Visible = False
            cbNormalBounceTop.Visible = False
            lblNormalBounceLikelyhood.Visible = False
            tbNormalBounceLikelyhood.Visible = False
        End If
    End Sub

    Private Sub cbNormalBounceTop_CheckedChanged(sender As Object, e As EventArgs) Handles cbNormalBounceTop.CheckedChanged
        If cbNormalBounceTop.Checked Then

        Else
            cbNormalBounceBottom.Checked = True
        End If
    End Sub

    Private Sub cbNormalBounceBottom_CheckedChanged(sender As Object, e As EventArgs) Handles cbNormalBounceBottom.CheckedChanged
        If cbNormalBounceBottom.Checked Then

        Else
            cbNormalBounceTop.Checked = True
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SaveFileDialog1.Filter = "FunScript (*.FunScript)|*.funscript|All files (*.*)|*.*"
        SaveFileDialog1.FilterIndex = 1
        SaveFileDialog1.RestoreDirectory = True

        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            IO.File.WriteAllText(SaveFileDialog1.FileName, tbOutput.Text)
        End If
    End Sub

    Private Sub tbLength_TextChanged(sender As Object, e As EventArgs) Handles tbLength.TextChanged
        'If CInt(tbLength.Text) > 100 Then
        '    lblDemo.Text = "Tech Demo Script Length Limited To 5 Mins.  Please let me know if you like the software and i will keep working on it."
        '    lblDemo.Visible = True
        'Else
        '    lblDemo.Visible = False
        'End If
    End Sub

#End Region
End Class

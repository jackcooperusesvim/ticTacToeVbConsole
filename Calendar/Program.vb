Imports System
Imports System.Net
Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions

Module Program
    Public Enum Piece
        X
        O
        Blank
    End Enum
    Public Enum Status
        Success
        Failure
    End Enum
    Public Structure Result
        Public Status As Status
        Public ErrorMessage As String
        Public Result As Object
        Public Function Unwrap()
            Select Case Status
                Case Status.Failure
                    Throw New Exception($"Unwrap Failed: {ErrorMessage}")
                Case Status.Success
                    Return Result
            End Select
        End Function
    End Structure
    Public Structure Turn
        Public x As Integer
        Public y As Integer
        Public piece As Piece
    End Structure
    Private Enum WinningPiece
        X
        O
        Lock
        Unknown
    End Enum
    Public Function Flip(p As Piece)
        If p = Piece.O Then
            Return Piece.X

        ElseIf p = Piece.X Then
            Return Piece.O
        Else
            Return Piece.Blank
        End If
    End Function

    Private Function ToPiece(tw As WinningPiece)
        If tw = WinningPiece.X Then
            Return Piece.X
        End If
        If tw = WinningPiece.O Then
            Return Piece.O
        End If
        Return Piece.Blank
    End Function

    Private Function ToTurnWinner(p As Piece)
        If p = Piece.X Then
            Return WinningPiece.X
        End If
        If p = Piece.O Then
            Return WinningPiece.O
        End If
        Return WinningPiece.Lock
    End Function

    Private Function AddPiece(wp As WinningPiece, p As Piece)
        If p = Piece.Blank Or wp = WinningPiece.Lock Then
            Return WinningPiece.Lock
        End If
        If wp = WinningPiece.Unknown Then
            Return ToTurnWinner(p)
        End If
        If ToPiece(wp) = p Then
            Return wp
        Else
            Return WinningPiece.Lock
        End If
    End Function
    Private Function ConcatWinningPieces(wps As WinningPiece())
        Dim Winner As WinningPiece = WinningPiece.Lock
        For Each wp In wps
            If wp <> WinningPiece.Lock And wp <> WinningPiece.Unknown Then
                If Winner <> wp Then
                    Return New Result With {.Status = Status.Failure, .ErrorMessage = "Both Players Won"}
                End If
                Winner = wp
            End If
        Next
        Return New Result With {.Status = Status.Success, .Result = Winner}
    End Function

    Public Class Board
        Private Const blank = Piece.Blank
        Private Const x = Piece.X
        Private Const y = Piece.O

        Public CurrentTurn As Piece
        Public Board(2, 2) As Piece

        Sub New(Optional IsBlank As Boolean = True, Optional FirstTurn As Piece = Piece.X)
            Board = {{blank, blank, blank}, {blank, blank, blank}, {blank, blank, blank}}
            CurrentTurn = FirstTurn
        End Sub

        Function PlayTurn(TurnToApply As Turn)
            If TurnToApply.piece = CurrentTurn Then
                If Board(TurnToApply.x, TurnToApply.y) = blank Then
                    Board(TurnToApply.x, TurnToApply.y) = TurnToApply.piece
                    Return New Result With {.Result = Status.Success}
                Else
                    Return New Result With {.Result = Status.Failure, .ErrorMessage = "The chosen spot is taken"}
                End If
            Else
                Return New Result With {.Result = Status.Failure, .ErrorMessage = $"Turn with Piece '{TurnToApply.piece}' Does Not Match Current Turn of '{CurrentTurn}'"}
            End If
        End Function

        Function IsWon()
            Dim WinningRows(2) As WinningPiece
            WinningRows = {WinningPiece.Unknown, WinningPiece.Unknown, WinningPiece.Unknown}

            Dim WinningCols(2) As WinningPiece
            WinningCols = {WinningPiece.Unknown, WinningPiece.Unknown, WinningPiece.Unknown}

            Dim WinningDiags(1) As WinningPiece
            WinningDiags = {WinningPiece.Unknown, WinningPiece.Unknown}

            For row As Integer = 0 To 2
                For col As Integer = 0 To 2
                    WinningRows(col) = AddPiece(WinningRows(col), Board(col, row))
                    WinningCols(row) = AddPiece(WinningRows(row), Board(col, row))
                    If col = row Then
                        WinningDiags(0) = AddPiece(WinningDiags(0), Board(col, row))
                    End If
                    If col = 2 - row Then
                        WinningDiags(1) = AddPiece(WinningDiags(1), Board(col, row))
                    End If
                Next
            Next
        End Function
        Function ValidTurns()

        End Function
    End Class
    Sub Main(args As String())
        Console.WriteLine("Hello World!")

    End Sub
End Module

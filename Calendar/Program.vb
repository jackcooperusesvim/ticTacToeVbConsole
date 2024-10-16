Imports System
Imports System.Runtime.CompilerServices

Module Program
    Public Enum Piece
        X
        O
        Blank
    End Enum
    Public Enum ResultEnum
        Success
        Failure
    End Enum
    Public Structure Result
        Public Result As ResultEnum
        Public Message As String
    End Structure
    Public Structure Turn
        Public x As Integer
        Public y As Integer
        Public piece As Piece
    End Structure
    Private Enum TurnWinner
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

    Private Function ToPiece(tw As TurnWinner)
        If tw = TurnWinner.X Then
            Return Piece.X
        End If
        If tw = TurnWinner.O Then
            Return Piece.O
        End If
        Return Piece.Blank
    End Function

    Private Function ToTurnWinner(p As Piece)
        If p = Piece.X Then
            Return TurnWinner.X
        End If
        If p = Piece.O Then
            Return TurnWinner.O
        End If
        Return TurnWinner.Lock
    End Function

    Private Function AddPiece(tw As TurnWinner, p As Piece)
        If p = Piece.Blank Or tw = TurnWinner.Lock Then
            Return TurnWinner.Lock
        End If
        If tw = TurnWinner.Unknown Then
            Return ToTurnWinner(p)
        End If
        If ToPiece(tw) = p Then

        Else
        End If
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
                    Return New Result With {.Result = ResultEnum.Success}
                Else
                    Return New Result With {.Result = ResultEnum.Failure, .Message = "The chosen spot is taken"}
                End If
            Else
                Return New Result With {.Result = ResultEnum.Failure, .Message = $"Turn with Piece '{TurnToApply.piece}' Does Not Match Current Turn of '{CurrentTurn}'"}
            End If
        End Function

        Function IsWon()
            Dim WinningRows(2) As Piece
            Dim WinningCols(2) As Piece
            Dim WinningDiags(2) As Piece
        End Function
        Function ValidTurns()

        End Function
    End Class
    Sub Main(args As String())
        Console.WriteLine("Hello World!")

    End Sub
End Module

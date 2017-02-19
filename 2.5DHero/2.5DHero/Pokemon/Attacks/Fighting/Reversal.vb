﻿Imports P3D.Legacy.Core.Pokemon
Imports P3D.Legacy.Core.Screens

Namespace BattleSystem.Moves.Fighting

    Public Class Reversal

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Fighting)
            Me.ID = 179
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Cool
            Me.Name = "Reversal"
            Me.Description = "An all-out attack that becomes more powerful the less HP the user has."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = True
            Me.ProtectAffected = True
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = True
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End
        End Sub

        Public Overrides Function GetBasePower(own As Boolean, BattleScreen As Screen) As Integer
            Dim screen As BattleScreen = BattleScreen
            Dim ownP As Pokemon = screen.OwnPokemon
            If own = False Then
                ownP = screen.OppPokemon
            End If

            Dim P As Double = (48 * ownP.HP) / ownP.MaxHP

            If P > 32 Then
                Return 20
            ElseIf P <= 32 And P >= 17 Then
                Return 40
            ElseIf P <= 16 And P >= 10 Then
                Return 80
            ElseIf P <= 9 And P >= 5 Then
                Return 100
            ElseIf P <= 4 And P >= 2 Then
                Return 150
            ElseIf P <= 1 Then
                Return 200
            End If

            Return 20
        End Function

    End Class

End Namespace
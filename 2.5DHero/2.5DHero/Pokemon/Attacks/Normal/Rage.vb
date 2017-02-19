﻿Imports P3D.Legacy.Core.Pokemon
Imports P3D.Legacy.Core.Screens

Namespace BattleSystem.Moves.Normal

    Public Class Rage

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 99
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 20
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = "Rage"
            Me.Description = "As long as this move is in use, the power of rage raises the Attack stat each time the user is hit in battle."
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
            Me.RemovesFrozen = False
            Me.HasSecondaryEffect = False

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

        Public Overrides Sub MoveSelected(own As Boolean, BattleScreen As Screen)
            Dim screen as BattleScreen = BattleScreen
            If own = True Then
                If Not screen.FieldEffects.OwnLastMove Is Nothing Then
                    If screen.FieldEffects.OwnLastMove.ID <> Me.ID Then
                        screen.FieldEffects.OwnRageCounter = 0
                    End If
                Else
                    screen.FieldEffects.OwnRageCounter = 0
                End If
            Else
                If Not screen.FieldEffects.OppLastMove Is Nothing Then
                    If screen.FieldEffects.OppLastMove.ID <> Me.ID Then
                        screen.FieldEffects.OppRageCounter = 0
                    End If
                Else
                    screen.FieldEffects.OppRageCounter = 0
                End If
            End If
        End Sub

        Public Overloads Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            If own = True Then
                If BattleScreen.FieldEffects.OwnRageCounter = 0 Then
                    BattleScreen.FieldEffects.OwnRageCounter = 1
                End If
            Else
                If BattleScreen.FieldEffects.OppRageCounter = 0 Then
                    BattleScreen.FieldEffects.OppRageCounter = 1
                End If
            End If
        End Sub

        Public Overrides Function GetBasePower(own As Boolean, BattleScreen As Screen) As Integer
            Dim screen as BattleScreen = BattleScreen
            Dim addPower As Integer = 0

            If own = True Then
                addPower = (screen.FieldEffects.OwnRageCounter.Clamp(1, 9) - 1) * 10
            Else
                addPower = (screen.FieldEffects.OppRageCounter.Clamp(1, 9) - 1) * 10
            End If

            Return Me.Power + addPower
        End Function

    End Class

End Namespace
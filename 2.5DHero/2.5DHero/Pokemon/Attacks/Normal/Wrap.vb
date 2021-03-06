﻿Imports P3D.Legacy.Core
Imports P3D.Legacy.Core.Pokemon

Namespace BattleSystem.Moves.Normal

    Public Class Wrap

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 35
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 15
            Me.Accuracy = 90
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = "Wrap"
            Me.Description = "A long body or vines are used to wrap and squeeze the target for four to five turns."
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

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.Trap
        End Sub

        Public Overloads Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim turns As Integer = 4
            If Core.Random.Next(0, 100) < 50 Then
                turns = 5
            End If

            If Not p.Item Is Nothing Then
                If p.Item.Name.ToLower() = "grip claw" And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                    turns = 5
                End If
            End If

            If own = True Then
                If BattleScreen.FieldEffects.OppWrap = 0 Then
                    BattleScreen.FieldEffects.OppWrap = turns
                    BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " was wrapped by " & p.GetDisplayName() & "!"))
                End If
            Else
                If BattleScreen.FieldEffects.OwnWrap = 0 Then
                    BattleScreen.FieldEffects.OwnWrap = turns
                    BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " was wrapped by " & p.GetDisplayName() & "!"))
                End If
            End If
        End Sub

    End Class

End Namespace
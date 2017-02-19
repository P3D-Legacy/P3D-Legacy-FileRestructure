Imports P3D.Legacy.Core.Pokemon
Imports P3D.Legacy.Core.Pokemon.Items
Imports P3D.Legacy.Core.Screens

Namespace BattleSystem.Moves.Normal

    Public Class NaturalGift

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 363
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Natural Gift"
            Me.Description = "The user draws power to attack by using its held Berry. The Berry determines the move's type and power."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = True
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = True
            Me.CounterAffected = False

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
            Dim p As Pokemon = screen.OwnPokemon
            If own = False Then
                p = screen.OppPokemon
            End If

            Dim itemID As Integer = 0
            If Not p.Item Is Nothing Then
                itemID = p.Item.Id
            End If

            If 1999 < itemID And itemID < 2016 Then
                Return 60
            ElseIf 2034 < itemID And itemID < 2052 Then
                Return 60
            ElseIf 2015 < itemID And itemID < 2031 Then
                Return 70
            ElseIf 2031 < itemID And itemID < 2036 Then
                Return 80
            ElseIf 2051 < itemID And itemID < 2064 Then
                Return 80
            Else
                Return 0
            End If
        End Function

        Public Overrides Function GetAttackType(own As Boolean, BattleScreen As Screen) As Element
            Dim screen As BattleScreen = BattleScreen
            Dim p As Pokemon = screen.OwnPokemon
            If own = False Then
                p = screen.OppPokemon
            End If

            If Not p.Item Is Nothing Then
                If p.Item.IsBerry = True Then
                    Return New Element(CType(p.Item, Berry).Type)
                End If
            End If
            Return New Element(Element.Types.Normal)
        End Function

        Public Overrides Function MoveFailBeforeAttack(Own As Boolean, BattleScreen As Screen) As Boolean
            Dim screen as BattleScreen = BattleScreen
            Dim p As Pokemon = screen.OwnPokemon
            Dim op As Pokemon = screen.OppPokemon
            If Own = False Then
                p = screen.OppPokemon
                op = screen.OwnPokemon
            End If

            If p.Item Is Nothing Then
                Return True
            Else
                If p.Item.isBerry = False Then
                    Return True
                End If
            End If

            If op.Ability.Name.ToLower() = "unnerve" And screen.FieldEffects.CanUseAbility(Not Own, screen) = True Then
                Return True
            End If

            If screen.FieldEffects.CanUseItem(Own) = False Or screen.FieldEffects.CanUseOwnItem(Own, screen) = False Then
                Return True
            End If

            Return False
        End Function

        Public Overloads Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            BattleScreen.Battle.RemoveHeldItem(own, own, BattleScreen, "", "move:naturalgift")
        End Sub

    End Class

End Namespace
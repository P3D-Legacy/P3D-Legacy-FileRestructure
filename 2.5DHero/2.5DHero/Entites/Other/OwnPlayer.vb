﻿Imports System.Drawing
Imports P3D.Legacy.Core
Imports P3D.Legacy.Core.Entities
Imports P3D.Legacy.Core.Entities.Other
Imports P3D.Legacy.Core.GameJolt
Imports P3D.Legacy.Core.GameJolt.Profiles
Imports P3D.Legacy.Core.Resources
Imports P3D.Legacy.Core.Resources.Models
Imports P3D.Legacy.Core.Screens

Public Class OwnPlayer
    Inherits BaseOwnPlayer

    Public Overrides Property UsingGameJoltTexture As Boolean

    Public Overrides Property SkinName As String
        Get
            Return _skinName
        End Get
        Set
            _skinName = Value
        End Set
    End Property

    Public Overrides Property DoAnimation As Boolean
        Get
            Return _doAnimation
        End Get
        Set
            _doAnimation = Value
        End Set
    End Property

    Public Shared ReadOnly AllowedSkins() As String = {"Ethan", "Lyra", "Nate", "Rosa", "Hilbert", "Hilda", "GoldRetro"}
    Private _doAnimation As Boolean = True
    Private _skinName As String = "Hilbert"

    Public Overrides Property Texture As Texture2D

    Public HasPokemonTexture As Boolean = False

    Dim lastRectangle As New Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0)
    Dim lastTexture As String = ""

    Dim AnimationX As Integer = 1
    Const AnimationDelayLenght As Single = 1.1F
    Dim AnimationDelay As Single = AnimationDelayLenght

    Public Sub New(ByVal X As Single, ByVal Y As Single, ByVal Z As Single, ByVal Textures() As Texture2D, ByVal TextureID As String, ByVal Rotation As Integer, ByVal ActionValue As Integer, ByVal AdditionalValue As String, ByVal Name As String, ByVal ID As Integer)
        MyBase.New(X, Y, Z, "OwnPlayer", Textures, {0, 0}, False, 0, New Vector3(1.0F), BaseModel.BillModel, 0, "", New Vector3(1.0F))

        SetTexture(TextureID, True)

        Me.NeedsUpdate = True
        Me.CreateWorldEveryFrame = True

        Me.DropUpdateUnlessDrawn = False
    End Sub

    Public Overrides Sub SetTexture(ByVal TextureID As String, ByVal UseGameJoltID As Boolean)
        Me.SkinName = TextureID
        HasPokemonTexture = False

        Dim texturePath As String = "Textures\NPC\"
        Dim isPokemon As Boolean = False
        If TextureID.StartsWith("[POKEMON|N]") = True Or TextureID.StartsWith("[Pokémon|N]") = True Then
            TextureID = TextureID.Remove(0, 11)
            isPokemon = True
            texturePath = "Pokemon\Overworld\Normal\"
            HasPokemonTexture = True
        ElseIf TextureID.StartsWith("[POKEMON|S]") = True Or TextureID.StartsWith("[Pokémon|S]") = True Then
            TextureID = TextureID.Remove(0, 11)
            isPokemon = True
            texturePath = "Pokemon\Overworld\Shiny\"
            HasPokemonTexture = True
        End If

        Dim PokemonAddition As String = ""
        If IsNumeric(TextureID) = True And texturePath.StartsWith("Pokemon\Overworld\") = True Then
            PokemonAddition = PokemonForms.GetDefaultOverworldSpriteAddition(CInt(TextureID))
        End If

        If Core.Player.IsGamejoltSave = True Then
            If texturePath & TextureID & PokemonAddition = "Textures\NPC\" & Emblem.GetPlayerSpriteFile(Emblem.GetPlayerLevel(Core.GameJoltSave.Points), Core.GameJoltSave.GameJoltID, Core.GameJoltSave.Gender) Then
                UseGameJoltID = True
            End If
        End If

        If UseGameJoltID = True And Core.Player.IsGamejoltSave = True And API.LoggedIn = True AndAlso Not Emblem.GetOnlineSprite(Core.GameJoltSave.GameJoltID) Is Nothing Then
            Logger.Debug("Change player texture to the online sprite.")
            Me.Texture = Emblem.GetOnlineSprite(Core.GameJoltSave.GameJoltID)
            UsingGameJoltTexture = True
        Else
            Logger.Debug("Change player texture to [" & texturePath & TextureID & PokemonAddition & "]")

            Me.Texture = TextureManager.GetTexture(texturePath & TextureID & PokemonAddition)
            UsingGameJoltTexture = False
        End If
    End Sub

    Protected Overrides Function CalculateCameraDistance(CPosition As Vector3) As Single
        Return MyBase.CalculateCameraDistance(CPosition) - 0.2F
    End Function

    Public Overrides Sub UpdateEntity()
        If Not Core.CurrentScreen Is Nothing Then
            If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                If Screen.Camera.Name = "Overworld" Then
                    Dim c As OverworldCamera = CType(Screen.Camera, OverworldCamera)
                    Me.Position = New Vector3(c.Position.X, c.Position.Y - 0.1F, c.Position.Z)
                End If
            End If
            If Me.Rotation.Y <> Screen.Camera.Yaw Then
                Me.Rotation.Y = Screen.Camera.Yaw
            End If
        End If

        Move()
        ChangeTexture()

        MyBase.UpdateEntity()
    End Sub

    Private Sub Move()
        If Screen.Camera.IsMoving() = True And Me.DoAnimation = True Then
            Me.AnimationDelay -= 0.13F
            If AnimationDelay <= 0.0F Then
                AnimationDelay = GetAnimationDelay()
                AnimationX += 1
                If HasPokemonTexture = True Then
                    If AnimationX > 2 Then
                        AnimationX = 1
                    End If
                Else
                    If AnimationX > 4 Then
                        AnimationX = 1
                    End If
                End If
            End If
        Else
            AnimationX = 1
            AnimationDelay = GetAnimationDelay()
            ChangeTexture()
        End If
    End Sub

    Public Overrides Sub ChangeTexture()
        If Not Me.Texture Is Nothing Then
            Dim r As New Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0)
            Dim cameraRotation As Integer = 0
            Dim spriteIndex As Integer = 0

            spriteIndex = 0

            If Screen.Camera.Name = "Overworld" Then
                spriteIndex = Screen.Camera.GetPlayerFacingDirection() - Screen.Camera.GetFacingDirection()
                While spriteIndex > 3
                    spriteIndex -= 4
                End While
                While spriteIndex < 0
                    spriteIndex += 4
                End While
            End If

            Dim frameSize As New Size(CInt(Me.Texture.Width / 3), CInt(Me.Texture.Height / 4))

            Dim x As Integer = 0
            If Screen.Camera.IsMoving() = True Then
                x = GetAnimationX() * frameSize.Width
            End If

            r = New Microsoft.Xna.Framework.Rectangle(x, frameSize.Width * spriteIndex, frameSize.Width, frameSize.Height)

            If r <> lastRectangle Or lastTexture <> SkinName Then
                lastRectangle = r
                lastTexture = SkinName
                Core.Player.Skin = SkinName

                Try
                    Dim t As Texture2D = TextureManager.GetTexture(Me.Texture, r, 1)
                    Textures(0) = t
                Catch
                    Logger.Log(Logger.LogTypes.Warning, "OwnPlayer.vb: Error assigning a new texture to the player.")
                End Try
            End If
        End If
    End Sub

    Private Function GetAnimationX() As Integer
        If HasPokemonTexture = True Then
            Return AnimationX
        Else
            Select Case AnimationX
                Case 1
                    Return 0
                Case 2
                    Return 1
                Case 3
                    Return 0
                Case 4
                    Return 2
            End Select
        End If

        Return 1
    End Function

    Public Overrides Sub Render()
        If InCameraFocus() = True Then
            Dim state = Core.GraphicsDevice.DepthStencilState
            Core.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead
            Draw(Me.Model, Me.Textures, True)
            Core.GraphicsDevice.DepthStencilState = state
        End If
    End Sub

    Friend Function InCameraFocus() As Boolean
        If Screen.Camera.Name = "Overworld" Then
            Dim c = CType(Screen.Camera, OverworldCamera)

            If c.CameraFocusType = OverworldCamera.CameraFocusTypes.Player And c.ThirdPerson = True Or c.CameraFocusType <> OverworldCamera.CameraFocusTypes.Player Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Overrides Sub ApplyShaders()
        Me.Shaders.Clear()
        For Each Shader As Shader In Screen.Level.Shaders
            Shader.ApplyShader({Me})
        Next
    End Sub

    Private Function GetAnimationDelay() As Single
        If Core.Player.IsRunning = True Then
            Return OwnPlayer.AnimationDelayLenght / 1.4F
        End If
        Return OwnPlayer.AnimationDelayLenght
    End Function
End Class
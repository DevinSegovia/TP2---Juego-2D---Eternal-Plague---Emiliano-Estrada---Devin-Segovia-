using System;
using Microsoft.Xna.Framework;
using EternalPlague.Scenes;
using MonoGameLibrary;
namespace Eternal_Plague;

public class Game1 : Core
{
    
    public Game1() : base("Eternal Plague", 1280, 832, false)
    {

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        base.Initialize();
        ChangeScene(new TitleScene());
    }

    protected override void LoadContent()
    {
    }
}

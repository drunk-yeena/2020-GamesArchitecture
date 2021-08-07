﻿using System;

namespace OpenGL_Game.Components
{
    [FlagsAttribute]
    enum ComponentTypes {
        COMPONENT_NONE     = 0,
	    COMPONENT_POSITION = 1 << 0,
        COMPONENT_VELOCITY = 1 << 1,
        COMPONENT_GEOMETRY = 1 << 2,
        COMPONENT_TEXTURE  = 1 << 3,
        COMPONENT_AUDIO = 1 << 4,
        COMPONENT_COLLISION_SPHERE = 1 << 5,
        COMPONENT_COLLISION_LINE = 1 << 6
    }

    interface IComponent
    {
        ComponentTypes ComponentType
        {
            get;
        }
    }
}

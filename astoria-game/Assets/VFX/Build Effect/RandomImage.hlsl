VFXSampler2D TexturePass1(in VFXSampler2D Smoke1, in VFXSampler2D Smoke2, in int Selector)
{
    VFXSampler2D Smoke;
    if (Selector == 0)
    {
        Smoke = Smoke1;
    } else if (Selector == 1)
    {
        Smoke = Smoke2;
    }
        

    return Smoke;
}

VFXSampler2D TexturePass2(in VFXSampler2D Smoke3, in VFXSampler2D Smoke4, in int Selector)
{
    VFXSampler2D Smoke;
    if (Selector == 2)
    {
        Smoke = Smoke3;
    } else if (Selector == 3)
    {
        Smoke = Smoke4;
    }

    return Smoke;
}

VFXSampler2D TextureFinal(in VFXSampler2D pass1, in VFXSampler2D pass2, in int Selector)
{
    VFXSampler2D Smoke;
    if (Selector > 1)
    {
        Smoke = pass2;
    } else
    {
        Smoke = pass1;
    }

    return Smoke;
}
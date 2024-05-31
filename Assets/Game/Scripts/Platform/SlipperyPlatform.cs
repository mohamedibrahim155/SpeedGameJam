
public class StaticPlatform : BasePlatform
{
    public override void Initialize() 
    {
        m_PlatformType = EPLatformType.STATIC;
    }
    public override void UpdateMovement()
    {
       
    }


    public override bool IsSlippery()
    {
        return base.IsSlippery();
    }

}

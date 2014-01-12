using UnityEngine;
using System.Collections;
public class Platform : MonoBehaviour
{
	public enum PlatformColorType {Empty, Type1, Type2}
	public PlatformColorType CurrentPlatformType = PlatformColorType.Empty;
	public int PositionX;
	public int PositionY;
	public void ChangeMyType(PlatformColorType ChangePlatformType)
	{
		CurrentPlatformType = ChangePlatformType;
	}
}
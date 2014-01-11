using UnityEngine;
using System.Collections;
public class Platform : MonoBehaviour
{
	public enum PlatformType {Empty, Type1, Type2}
	public PlatformType CurrentPlatformType = PlatformType.Empty;
	public void ChangeMyType(PlatformType ChangePlatformType)
	{
		CurrentPlatformType = ChangePlatformType;
	}
}
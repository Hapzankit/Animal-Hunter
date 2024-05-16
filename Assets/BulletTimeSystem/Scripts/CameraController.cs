using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera; // Assign your main camera in the inspector
    public Camera[] otherCameras; // Assign other cameras in the inspector

    private int currentCameraIndex = -1;

    public void ChangeCameraAngle()
    {
        // Only shuffle and choose a camera if there are at least 2 cameras
        if (otherCameras.Length > 1)
        {
            currentCameraIndex = (currentCameraIndex + 1) % otherCameras.Length;

            // If starting a new cycle, shuffle the cameras array
            if (currentCameraIndex == 0)
            {
                ShuffleCameras();
            }

            // Activate the selected camera
            otherCameras[currentCameraIndex].gameObject.SetActive(true);

            // Optionally, deactivate the previously active camera
            // Ensure we don't disable the camera we just enabled
            if (currentCameraIndex > 0)
            {
                otherCameras[currentCameraIndex - 1].gameObject.SetActive(false);
            }
            else
            {
                otherCameras[otherCameras.Length - 1].gameObject.SetActive(false);
            }

            print("Camera changed to index: " + currentCameraIndex);
        }
    }

    void ShuffleCameras()
    {
        for (int i = 0; i < otherCameras.Length; i++)
        {
            Camera temp = otherCameras[i];
            int randomIndex = Random.Range(i, otherCameras.Length);
            otherCameras[i] = otherCameras[randomIndex];
            otherCameras[randomIndex] = temp;
        }
    }
}

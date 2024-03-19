using UnityEngine;

[CreateAssetMenu(menuName = "VariantSOs/Room Variants")]
public class RoomVariantsSO : VariantSO<Room>
{
    private Room lastRoom = null;



    public override Room SelectRandom()
    {
        Room selectedRoom = variants[Random.Range(0, variants.Count)];
        if (lastRoom == null)
        {
            lastRoom = selectedRoom;
        }

        else
        {
            while (lastRoom == selectedRoom)
            {
                selectedRoom = variants[Random.Range(0, variants.Count)];
            }
            lastRoom = selectedRoom;
        }

        return selectedRoom;
    }
}
using UnityEngine;

public class InstrumentOutput : MonoBehaviour, IClickable {

    public Synth.ContinuousNotePlayer notePlayer;
    public Color normalBoxColor;
    public Color activeBoxColor;
    public Color normalPlayButtonColor;
    public Color activePlayButtonColor;
    public SpriteRenderer box;
    public SpriteRenderer playButton;

    void IClickable.OnClick() {
        ToggleNotePlaying();
    }

    void IClickable.OnDoubleClick() {
        ToggleNotePlaying();
    }

    void ToggleNotePlaying() {
        notePlayer.Toggle();
        box.color = notePlayer.isPlaying ? activeBoxColor : normalBoxColor;
        playButton.color = notePlayer.isPlaying ? activePlayButtonColor : normalPlayButtonColor;
    }
}

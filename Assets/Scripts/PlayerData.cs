using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string deckName;
    public IList<Card> deck;
    public BasicColor themeColor; 
}

// this class is a simplification of the Color class that allows it to be serialized into JSON
// the standard Color class cannot be serialized because it contains a proprty of type Color, leading to infinite self-references
public class BasicColor
{
    public float red;
    public float green;
    public float blue;
    public float alpha;

    // constructor
    public BasicColor(float r, float g, float b, float a) {
        red = r;
        green = g;
        blue = b;
        alpha = a;
    }
    // allows this to be implicitly cast and treated as a Color
    public static implicit operator Color(BasicColor color) {
        return new Color(color.red, color.green, color.blue, color.alpha);
    }
    // converts Color to BasicColor
    public static BasicColor ConvertToBasicColor(Color color) {
        return new BasicColor(color.r, color.g, color.b, color.a);
    }
}

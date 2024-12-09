using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.IO;
using Newtonsoft.Json;
using TMPro;

public class BasicColorTest
{
    [UnityTest]

    // Tests converting between Color and Basic Color works as expected
    public IEnumerator ConversionTest()
    {
        Color color = Color.blue;
        BasicColor basic = new(color.r, color.g, color.b, color.a);
        Assert.AreNotEqual(color, basic);
        Assert.AreEqual(color, (Color)basic);
        Assert.AreEqual(BasicColor.ConvertToBasicColor(color), basic);
        basic = new(1,2,3,4);
        Assert.AreNotEqual(color, basic);
        Assert.AreNotEqual(color, (Color)basic);
        Assert.AreNotEqual(BasicColor.ConvertToBasicColor(color), basic);
        color = new(basic.red, basic.green, basic.blue, basic.alpha);
        Assert.AreNotEqual(color, basic);
        Assert.AreEqual(color, (Color)basic);
        Assert.AreEqual(BasicColor.ConvertToBasicColor(color), basic);
        return null;
    }
    [UnityTest]
    // Tests that BasicColor can be converted to JSON, unlike Color
    public IEnumerator JSONTest()
    {
        Color color = Color.green;
        BasicColor basic = new(color.r, color.g, color.b, color.a);
        Assert.Throws<JsonSerializationException>(() => JsonConvert.SerializeObject(color));
        Assert.DoesNotThrow(() => JsonConvert.SerializeObject(basic));
        string jsonBasic = JsonConvert.SerializeObject(basic);
        BasicColor deserializedBasic = JsonConvert.DeserializeObject<BasicColor>(jsonBasic);
        Assert.AreEqual(basic, deserializedBasic);
        Assert.AreEqual(color, (Color)deserializedBasic);
        Assert.AreEqual(BasicColor.ConvertToBasicColor(color), deserializedBasic);
        return null;
    }
}

package com.fit.model;

import com.fit.model.path.IElementPath;

import java.awt.*;

public class ElementAttributes {
    private String designedName;
    private String designedId;
    private String elementType;
    private IElementPath elementPath;
    private String imageCaptureEncoded;
    private String helpText;
    private boolean isEnabled;
    private String name;
    private Rectangle rectBounding;

    public String getDesignedName() {
        return designedName;
    }

    public void setDesignedName(String designedName) {
        this.designedName = designedName;
    }

    public String getDesignedId() {
        return designedId;
    }

    public void setDesignedId(String designedId) {
        this.designedId = designedId;
    }

    public String getElementType() {
        return elementType;
    }

    public void setElementType(String elementType) {
        this.elementType = elementType;
    }

    public IElementPath getElementPath() {
        return elementPath;
    }

    public void setElementPath(IElementPath elementPath) {
        this.elementPath = elementPath;
    }

    public String getImageCaptureEncoded() {
        return imageCaptureEncoded;
    }

    public void setImageCaptureEncoded(String imageCaptureEncoded) {
        this.imageCaptureEncoded = imageCaptureEncoded;
    }

    public String getHelpText() {
        return helpText;
    }

    public void setHelpText(String helpText) {
        this.helpText = helpText;
    }

    public boolean isEnabled() {
        return isEnabled;
    }

    public void setEnabled(boolean enabled) {
        isEnabled = enabled;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public Rectangle getRectBounding() {
        return rectBounding;
    }

    public void setRectBounding(Rectangle rectBounding) {
        this.rectBounding = rectBounding;
    }
}

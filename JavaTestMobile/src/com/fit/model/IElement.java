package com.fit.model;

import java.awt.*;
import java.util.List;

public interface IElement {
    boolean click();
    String getText();
    String getTextPattern();
    double GetWidth();
    double GetHeight();
    Rectangle GetBoundingRect();
    boolean SetWidth(double width);
    boolean SetHeight(double height);
    boolean SetSize(double width, double height);

    List<IElement> getChildren();
    void setChildren(List<IElement> children);
    IElement getParent();
    void setParent(IElement parent);
    String getId();
    void setId(String id);
    ElementAttributes getAttributes();
    void setAttributes(ElementAttributes attributes);
    String getAlias();
    void setAlias(String alias);
    String GetNameorAlias();
}

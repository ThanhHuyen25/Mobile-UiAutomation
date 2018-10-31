package com.fit.model;

import java.util.List;

public abstract class ElementBase implements IElement{
    protected List<IElement> children;
    protected IElement getParent;
    protected String getId;
    protected ElementAttributes getAttributes;
    protected String getAlias;
    protected String GetNameorAlias;

    @Override
    public List<IElement> getChildren() {
        return children;
    }

    @Override
    public void setChildren(List<IElement> children) {
        this.children = children;
    }

    public IElement getGetParent() {
        return getParent;
    }

    public void setGetParent(IElement getParent) {
        this.getParent = getParent;
    }

    public String getGetId() {
        return getId;
    }

    public void setGetId(String getId) {
        this.getId = getId;
    }

    public ElementAttributes getGetAttributes() {
        return getAttributes;
    }

    public void setGetAttributes(ElementAttributes getAttributes) {
        this.getAttributes = getAttributes;
    }

    public String getGetAlias() {
        return getAlias;
    }

    public void setGetAlias(String getAlias) {
        this.getAlias = getAlias;
    }

    public String getGetNameorAlias() {
        return GetNameorAlias;
    }

    public void setGetNameorAlias(String getNameorAlias) {
        GetNameorAlias = getNameorAlias;
    }
}

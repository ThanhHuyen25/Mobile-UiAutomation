package com.fit.model.path;

public class ElementPath implements IElementPath {
    protected String path;
    public ElementPath(String path)
    {
        this.path = path;
    }
    public final String CONNECT = ".";

    public ElementPath AppendWith(String strAppend)
    {
        return new ElementPath(CONNECT + strAppend);
    }

    @Override
    public String getPath() {
        return path;
    }

    @Override
    public void setPath(String path) {
        this.path = path;
    }
}

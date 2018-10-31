package com.fit.model.path;

public interface IElementPath {
    // append new string to path
    IElementPath AppendWith(String strAppend);
    String getPath();
    void setPath(String path);
}

package com.fit.model.other;

import com.fit.model.IElement;

/**
 * represent for validation action
 */
public class Validate {
    /**
     * Validate existence of an element
     * @param element
     * @return
     */
    public static boolean exists(IElement element)
    {
        return false;
    }

    /**
     * Validate non-existence of an element
     * @param element
     * @return
     */
    public static boolean notExists(IElement element)
    {
        return false;
    }

    /**
     * Validate text of an element
     * @param element
     * @param value
     * @return
     */
    public static boolean textEquals(IElement element, String value)
    {
        return false;
    }

    /**
     * Validate text of an element
     * @param element
     * @param value
     * @return
     */
    public static boolean textNotEquals(IElement element, String value)
    {
        return false;
    }

    /**
     * Validate text of an element contain sth
     * @param element
     * @param value
     * @return
     */
    public static boolean textContains(IElement element, String value)
    {
        return false;
    }

    /**
     * Validate text of an element not contain sth
     * @param element
     * @param value
     * @return
     */
    public static boolean textNotContains(IElement element, String value)
    {
        return false;
    }

    /**
     * Validate width of an element equals with
     * @param element
     * @param value
     * @return
     */
    public static boolean widthEquals(IElement element, double value)
    {
        return false;
    }

    /**
     * Validate height of an element equals with
     * @param element
     * @param value
     * @return
     */
    public static boolean heightEquals(IElement element, double value)
    {
        return false;
    }

    /**
     * Validate visibility of an element
     * @param element
     * @param value
     * @return
     */
    public static boolean visible(IElement element, boolean value)
    {
        return false;
    }

    /**
     * capture screenshots
     * @return
     */
    public static String captureScreen()
    {
        return null;
    }
}

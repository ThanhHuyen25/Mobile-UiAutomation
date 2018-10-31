package com.fit.model.other;

import java.util.List;

/**
 * represent for Keyboard action
 */
public class Keyboard {
    /**
     * send single key
     * @param key
     * @return
     */
    public static boolean sendSingleKey(String key)
    {
        return false;
    }

    /**
     * send shortcut, e.g., Ctrl+A
     * @param keys
     * @return
     */
    public static boolean sendCombinedKeys(String... keys)
    {
        return false;
    }

    /**
     * the same above, but use list instead
     * @param listKeys
     * @return
     */
    public static boolean sendCombinedKeys(List<String> listKeys)
    {
        return false;
    }
}

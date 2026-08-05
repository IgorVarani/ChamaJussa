import React from "react";
import { StyleSheet, Text, TextProps } from "react-native";

export const CustomH1 = (props: TextProps) => {
    return (
        <Text style={styles.H1} {...props} />
    )
}

export const CustomSub = (props: TextProps) => {
    return (
        <Text style={styles.Sub} {...props} />
    )
}

const styles = StyleSheet.create({
    H1: {
        fontSize: 24,
        fontWeight: 'bold'
    },
    Sub: {
        fontSize: 16,
        fontWeight: '100'
    }
})
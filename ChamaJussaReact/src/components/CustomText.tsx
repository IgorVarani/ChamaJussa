import React from "react";
import { StyleSheet, Text, TextProps } from "react-native";
import { Colors } from "../constants/theme";

export const CustomH1 = (props: TextProps) => {
    return (
        <Text style={styles.h1} {...props} />
    )
}

export const CustomSub = (props: TextProps) => {
    return (
        <Text style={styles.sub} {...props} />
    )
}

export const CustomBtnText = (props: TextProps) => {
    return (
        <Text style={styles.btnText} {...props} />
    )
}

const styles = StyleSheet.create({
    h1: {
        fontSize: 24,
        fontWeight: 'bold'
    },
    sub: {
        fontSize: 16,
        fontWeight: '100',
        marginBottom: 20
    },
    btnText: {
        textAlign: 'center',
        color: Colors.BGColor,
        fontWeight: 'bold',
        fontSize: 16
    }
})
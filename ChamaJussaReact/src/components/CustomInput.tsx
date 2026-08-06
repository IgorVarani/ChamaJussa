import React from 'react';
import { TextInput, StyleSheet, TextInputProps, Text, TextProps } from 'react-native';

export const CustomInput = (props: TextInputProps) => {
    return (
        <TextInput
            style={styles.input}
            placeholderTextColor="#888"
            {...props}
        />
    );
};

export const CustomTitleInput = (props: TextProps) => {
    return (
        <Text style={styles.titleInput} {...props} />
    )
}

const styles = StyleSheet.create({
    input: {
        width: 250,
        height: 45,
        backgroundColor: '#F5F5F5',
        borderWidth: 1,
        borderColor: '#DDD',
        borderRadius: 8,
        paddingHorizontal: 12,
        marginBottom: 12,
        fontSize: 14,
        color: '#333',
    },
    titleInput: {
        marginBottom: 5,
        fontWeight: 'bold',
    }
});
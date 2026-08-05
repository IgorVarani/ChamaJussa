import React from 'react';
import { TextInput, StyleSheet, TextInputProps, Text } from 'react-native';

export const CustomInput = (props: TextInputProps) => {
    return (
        <TextInput
            style={styles.input}
            placeholderTextColor="#888"
            {...props}
        />
    );
};

export const CustomTitleInput = (props: Text) => {
    return (
        <Text style={styles.title} {...props} />
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
    
});
import React from 'react'
import { Text, StyleSheet, View, Image, TextInput, TouchableOpacity } from 'react-native'
import { CustomInput, CustomTitleInput } from '../../components/CustomInput'
import { CustomBtnText, CustomH1, CustomSub } from '../../components/CustomText'
import { Botao, Colors } from '../../constants/theme'

import Logo from '../../../assets/imgs/logo.svg'

export const Login = () => {
  return (
    <View style={styles.container}>
        <Logo />
        <View style={styles.containerForm}>
            <CustomH1>Chama Jussa</CustomH1>
            <CustomSub>Gerenciamento de Ordens de Serviço</CustomSub>
        <View style={styles.inputs}>
            <CustomTitleInput>E-Mail</CustomTitleInput>
                <CustomInput placeholder='email@email.com' />
            <CustomTitleInput>Senha</CustomTitleInput>
                <CustomInput placeholder='Digite sua senha' />
            <TouchableOpacity style={styles.btn}>
                <CustomBtnText>Acessar o Sistema</CustomBtnText>
            </TouchableOpacity>
            </View>
        </View>
    </View>
  )
}

const styles = StyleSheet.create({
    container:{
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center'
    },
    containerForm:{
        boxShadow: '0px 4px 6px rgba(0, 0, 0, 0.5)',
        borderRadius: 10,
        backgroundColor: 'white',
        width: 350,
        height: 350,
        paddingVertical: 10,
        paddingHorizontal: 10,
        justifyContent: 'center',
        alignItems: 'center'
    },
    inputs:{
    },
    btn:{
        backgroundColor: Colors.BtnVerde,
        ...Botao,
    },
})
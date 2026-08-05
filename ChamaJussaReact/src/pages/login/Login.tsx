import React from 'react'
import { Text, StyleSheet, View, Image, TextInput, TouchableOpacity } from 'react-native'
import { CustomInput } from '../../components/CustomInput'
import { CustomH1, CustomSub } from '../../components/CustomText'
import { Botao, Colors } from '../../constants/theme'

export const Login = () => {
  return (
    <View style={styles.container}>
        <Image source={require('../../../assets/imgs/logo.svg')}></Image>
        <View style={styles.containerForm}>
            <CustomH1>Chama Jussa</CustomH1>
            <CustomSub>Gerenciamento de Ordens de Serviço</CustomSub>
        <View style={styles.inputs}>
            <Text>E-Mail</Text>
                <CustomInput placeholder='email@email.com' />
            <Text>Senha</Text>
                <CustomInput placeholder='Digite sua senha' />
            <TouchableOpacity style={styles.btn}>Acessar o sistema</TouchableOpacity>
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
        ...Botao
    },
    botaoTxt:{
        
    }
})
﻿// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Lightup
{
    using System;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal struct RefTypeSyntaxWrapper : ISyntaxWrapper<TypeSyntax>
    {
        private const string RefTypeSyntaxTypeName = "Microsoft.CodeAnalysis.CSharp.Syntax.RefTypeSyntax";
        private static readonly Type RefTypeSyntaxType;

        private static readonly Func<TypeSyntax, SyntaxToken> RefKeywordAccessor;
        private static readonly Func<TypeSyntax, TypeSyntax> TypeAccessor;
        private static readonly Func<TypeSyntax, SyntaxToken, TypeSyntax> WithRefKeywordAccessor;
        private static readonly Func<TypeSyntax, TypeSyntax, TypeSyntax> WithTypeAccessor;

        private readonly TypeSyntax node;

        static RefTypeSyntaxWrapper()
        {
            RefTypeSyntaxType = typeof(CSharpSyntaxNode).GetTypeInfo().Assembly.GetType(RefTypeSyntaxTypeName);
            RefKeywordAccessor = LightupHelpers.CreateSyntaxPropertyAccessor<TypeSyntax, SyntaxToken>(RefTypeSyntaxType, nameof(RefKeyword));
            TypeAccessor = LightupHelpers.CreateSyntaxPropertyAccessor<TypeSyntax, TypeSyntax>(RefTypeSyntaxType, nameof(Type));
            WithRefKeywordAccessor = LightupHelpers.CreateSyntaxWithPropertyAccessor<TypeSyntax, SyntaxToken>(RefTypeSyntaxType, nameof(RefKeyword));
            WithTypeAccessor = LightupHelpers.CreateSyntaxWithPropertyAccessor<TypeSyntax, TypeSyntax>(RefTypeSyntaxType, nameof(Type));
        }

        private RefTypeSyntaxWrapper(TypeSyntax node)
        {
            this.node = node;
        }

        public TypeSyntax SyntaxNode => this.node;

        public SyntaxToken RefKeyword
        {
            get
            {
                return RefKeywordAccessor(this.SyntaxNode);
            }
        }

        public TypeSyntax Type
        {
            get
            {
                return TypeAccessor(this.SyntaxNode);
            }
        }

        public static explicit operator RefTypeSyntaxWrapper(SyntaxNode node)
        {
            if (node == null)
            {
                return default(RefTypeSyntaxWrapper);
            }

            if (!IsInstance(node))
            {
                throw new InvalidCastException($"Cannot cast '{node.GetType().FullName}' to '{RefTypeSyntaxTypeName}'");
            }

            return new RefTypeSyntaxWrapper((TypeSyntax)node);
        }

        public static implicit operator TypeSyntax(RefTypeSyntaxWrapper wrapper)
        {
            return wrapper.node;
        }

        public static bool IsInstance(SyntaxNode node)
        {
            return node != null && LightupHelpers.CanWrapNode(node, RefTypeSyntaxType);
        }

        public RefTypeSyntaxWrapper WithRefKeyword(SyntaxToken refKeyword)
        {
            return new RefTypeSyntaxWrapper(WithRefKeywordAccessor(this.SyntaxNode, refKeyword));
        }

        public RefTypeSyntaxWrapper WithType(TypeSyntax type)
        {
            return new RefTypeSyntaxWrapper(WithTypeAccessor(this.SyntaxNode, type));
        }
    }
}